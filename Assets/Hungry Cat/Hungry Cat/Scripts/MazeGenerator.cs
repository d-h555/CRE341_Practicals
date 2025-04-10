using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;

public class MazeGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject npcPrefab, waypointsPrefab;
    public GameObject groundObject;

    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    [SerializeField]
    private int _seed;

    [SerializeField]
    private bool _useSeed;
    private MazeCell[,] _mazeGrid;

    [SerializeField]
    private int maxAttempts = 1000;

    
	[SerializeField] 
    int numberOfNPCs = 5;
	[SerializeField] 
    List<GameObject> npcs = new List<GameObject>();
	[SerializeField] 
    int numberWaypoints = 4;
	[SerializeField] 
    List<GameObject> waypoints = new List<GameObject>();

    // int numberOfCheese = 10;
    // [SerializeField]
    // List<GameObject> Cheese = new List<GameObject>();


    void Start()
    {
        if (_useSeed)
        {
            Random.InitState(_seed);
        }
        else
        {
            int randomSeed = Random.Range(1, 1000000);
            Random.InitState(randomSeed);

            Debug.Log(randomSeed);
        }

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        GetComponent<NavMeshSurface>().BuildNavMesh();

        if (groundObject == null)
        {
            Debug.LogError("No object tagged 'Ground' found. Make sure your ground plane is tagged correctly.");
            return;
        }

        PlacePlayer();

        SpawnWayPoints(numberWaypoints);
        SpawnNPCs(numberOfNPCs);
        // SpawnCheese(numberOfCheese);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GenerateMaze(null, _mazeGrid[0, 0]);
            GetComponent<NavMeshSurface>().BuildNavMesh();
            PlacePlayer();

            // delete existing NPCs and spawn new ones
            GameObject[] go_npcs = GameObject.FindGameObjectsWithTag("NPC");
            foreach (GameObject npc in go_npcs) Destroy(npc);

            GameObject[] go_wps = GameObject.FindGameObjectsWithTag("Waypoint");
            foreach (GameObject wp in go_wps) Destroy(wp);

            SpawnWayPoints(numberWaypoints);
            SpawnNPCs(numberOfNPCs);
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.localPosition.z < currentCell.transform.localPosition.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.localPosition.z > currentCell.transform.localPosition.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void PlacePlayer()
    {
        Vector3 randomPlayerPos = GetRandomGroundPoint();
        player.transform.position = randomPlayerPos;
    }

    public Vector3 GetRandomGroundPoint()
    {
       Bounds groundBounds = groundObject.GetComponent<Renderer>().bounds;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randX = Random.Range(0, _mazeWidth);
            float randZ = Random.Range(0, _mazeDepth);

            Vector3 randomPoint = new Vector3(randX, 0, randZ);

            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, 1))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    return hit.point;
                }
            }
        }

        Debug.LogWarning("No valid 'Ground' point found.");
        return Vector3.zero;
    }

  private void SpawnNPCs(int count)
{
    for (int i = 0; i < count; i++)
    {
        Vector3 randomNPCPos = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound)
        {
            // Get a random cell within the maze boundaries
            int randomX = Random.Range(0, _mazeWidth);
            int randomZ = Random.Range(0, _mazeDepth);

            MazeCell randomCell = _mazeGrid[randomX, randomZ];
            randomNPCPos = randomCell.transform.position;

            // Ensure the position is valid on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomNPCPos, out hit, 1.0f, NavMesh.AllAreas))
            {
                randomNPCPos = hit.position;
                validPositionFound = true;
            }
        }

        GameObject npc = Instantiate(npcPrefab, randomNPCPos, Quaternion.identity);
        npc.tag = "NPC";
        Debug.Log("Generated NPCs");
    }
}
       private void SpawnWayPoints(int count)
{
    for (int i = 0; i < count; i++)
    {
        Vector3 randomWaypointPos = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound)
        {
            // Get a random cell within the maze boundaries
            int randomX = Random.Range(0, _mazeWidth);
            int randomZ = Random.Range(0, _mazeDepth);

            MazeCell randomCell = _mazeGrid[randomX, randomZ];
            randomWaypointPos = randomCell.transform.position;

            // Ensure the position is valid on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomWaypointPos, out hit, 1.0f, NavMesh.AllAreas))
            {
                randomWaypointPos = hit.position;
                validPositionFound = true;
            }
        }

        GameObject waypoint = Instantiate(waypointsPrefab, randomWaypointPos, Quaternion.identity);
        waypoint.tag = "Waypoint";
        Debug.Log("Generated waypoints");
         }
    }
}

    

    //         private void SpawnCheese (int count )    
            


    //         }
    //     }
    // }
