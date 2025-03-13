using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.AI.Navigation;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MazeBuilder : MonoBehaviour 
{

    public GameObject player; // Reference to your player prefab
    public GameObject npcPrefab, waypointsPrefab; // Reference to your NPC prefab
    public GameObject groundObject;
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 58)]
    public int randomFillPercent;

    [SerializeField] int numberOfNPCs = 5;
    [SerializeField] List<GameObject> npcs = new List<GameObject>();
    [SerializeField] int numberWaypoints = 4;
    [SerializeField] List<GameObject> waypoints = new List<GameObject>();

    int[,] map;

    [SerializeField] public NavMeshSurface surface;
    [SerializeField] private float raycastHeight = 50f; // Height above the plane from which to cast rays.
    [SerializeField] private int maxAttempts = 1000; // Safety limit to avoid an infinite loop.

    void Start() {
        if (groundObject == null) {
            Debug.LogError("No object tagged 'Ground' found. Make sure your ground plane is tagged correctly.");
            return;
        }

        GenerateMap();
        surface.BuildNavMesh();

        // After the NavMesh is generated/baked, place the player
        PlacePlayer();

        SpawnWayPoints(numberWaypoints);
        SpawnNPCs(numberOfNPCs);
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            GenerateMap();
            surface.BuildNavMesh();
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

    void GenerateMap() {
        map = new int[width, height];
        InitialiseMaze();
        CarveMaze(1, 1);

        int borderSize = 1;
        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++) {
            for (int y = 0; y < borderedMap.GetLength(1); y++) {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                } else {
                    borderedMap[x, y] = 1;
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);
    }

    void InitialiseMaze() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x % 2 == 1 && y % 2 == 1) {
                    map[x, y] = 0;
                } else {
                    map[x, y] = 1;
                }
            }
        }
    }

    void CarveMaze(int x, int y) {
        int[] directions = { 0, 1, 2, 3 };
        ShuffleArray(directions);

        foreach (int dir in directions) {
            int dx = 0, dy = 0;
            switch (dir) {
                case 0: dy = -2; break; // Up
                case 1: dx = 2; break;  // Right
                case 2: dy = 2; break;  // Down
                case 3: dx = -2; break; // Left
            }

            int nx = x + dx;
            int ny = y + dy;

            if (IsInMapRange(nx, ny) && map[nx, ny] == 1) {
                map[x + dx / 2, y + dy / 2] = 0; // Remove wall between
                map[nx, ny] = 0;
                CarveMaze(nx, ny);
            }
        }
    }

    void ShuffleArray(int[] array) {
        for (int i = array.Length - 1; i > 0; i--) {
            int j = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    bool IsInMapRange(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void PlacePlayer() {
        Vector3 randomPlayerPos = GetRandomGroundPoint();
        player.transform.position = randomPlayerPos;
    }

    Vector3 GetRandomGroundPoint() {
        Bounds groundBounds = groundObject.GetComponent<Renderer>().bounds;

        for (int i = 0; i < maxAttempts; i++) {
            float randX = Random.Range(groundBounds.min.x, groundBounds.max.x);
            float randZ = Random.Range(groundBounds.min.z, groundBounds.max.z);
            Vector3 origin = new Vector3(randX, raycastHeight, randZ);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, Mathf.Infinity)) {
                if (hit.collider.CompareTag("Ground")) {
                    return hit.point;
                }
            }
        }

        Debug.LogWarning("No valid 'Ground' point found.");
        return Vector3.zero;
    }

    void SpawnNPCs(int count) {
        for (int i = 0; i < count; i++) {
            Vector3 randomNPCPos = Vector3.zero;
            bool validPositionFound = false;
            int attempts = 0;

            while (!validPositionFound && attempts < maxAttempts) {
                randomNPCPos = GetRandomGroundPoint();
                if (randomNPCPos != Vector3.zero) {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomNPCPos, out hit, 1.0f, NavMesh.AllAreas)) {
                        randomNPCPos = hit.position;
                        validPositionFound = true;
                    }
                }
                attempts++;
            }

            if (validPositionFound) {
                Instantiate(npcPrefab, randomNPCPos, Quaternion.identity);
                npcs.Add(npcPrefab);
            } else {
                Debug.LogWarning("Failed to find a valid NavMesh point for NPC.");
            }
        }
    }

    void SpawnWayPoints(int count) {
        for (int i = 0; i < count; i++) {
            Vector3 randomNPCPos = Vector3.zero;
            bool validPositionFound = false;
            int attempts = 0;

            while (!validPositionFound && attempts < maxAttempts) {
                randomNPCPos = GetRandomGroundPoint();
                if (randomNPCPos != Vector3.zero) {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomNPCPos, out hit, 1.0f, NavMesh.AllAreas)) {
                        randomNPCPos = hit.position;
                        validPositionFound = true;
                    }
                }
                attempts++;
            }

            if (validPositionFound) {
                Instantiate(waypointsPrefab, randomNPCPos, Quaternion.identity);
                waypoints.Add(waypointsPrefab);
            } else {
                Debug.LogWarning("Failed to find a valid NavMesh point for Waypoint.");
            }
        }
    }
}
