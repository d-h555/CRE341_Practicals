using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
public class Mice : MonoBehaviour
{
    public static Mice Instance { get; private set; }
    public float maxSpeed;
    public float maxSight;
    public GameObject player;
    bool seesPlayer = false;

    NavMeshAgent agent;

    public float wanderSpeed;
    public float wanderTime;
    private float timeToChangeDirection = 0;
    public AnimationCurve fleeDistanceCurve;

    private Vector3 randomDirection;

    private GameObject playerPosition;

void start()
{
    waypoints = MazeGenerator.Instance.waypoints;
    
    for (int i = 0; i < numberWaypoints; i++)
    {
        waypoints[i] = waypointsPrefab;
    }
}

    // Update is called once per frame
    void Update()
    {
        if(PauseController.IsGamePaused || isWaiting)
        {
            return;
        }

        if (playerPosition == null)
        {
            playerPosition = GameObject.FindWithTag("Player");
        }

        MoveToWayPoint();
    }

    void MoveToWayPoint()
        {
            Transform target = waypoiny[currentWaypointIndex].transform;

            transform.positon = Vector2.MoveTowards(transform.position, moveSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                StartCoroutine(WaitWaypoint());
            }

            IEnumerator WaitWaypoint()
            {
                isWaiting = true;
                yield return new WaitForSeconds(waitTime);
                isWaiting = false;
            }

            //if looping is established, loop through the waypoints
            //if looping is not established, increment currentWaypointIndex by but dont exceed last waypoint
            {
                currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(CurrentWaypointIndex + 1, waypoints.Length -1);

                isWaiting = false;
            }
        }

        if (playerPosition ! = null)
        {
            Vector3 direction = transform.position - playerPosition.transform.position;
            Vector3 distance = transform.position - playerPosition.transform.position;

            if (distance.magnitude < maxSight)
            {
                Debug.Log("i can see player");
                seesPlayer = true;
                float distScaled = distance.magnitude/maxSight;
                float fleeDistance =   fleeDistanceCurve.Evaluate(distScaled);
                
                transform.position += direction.normalized * fleeDistance * fleeDistance * maxSpeed * Time.deltaTime;
            } 
            else
            {
                if (timeToChangeDirection <= 0)
                {
                    seesPlayer = false;
                    float z = UnityEngine.Random.Range(-1.0f, 1.0f);
                    float x = UnityEngine.Random.Range(-1.0f, 1.0f);
                    randomDirection = new Vector3(x, 0, z);
                    timeToChangeDirection = wanderTime;
                }
            }
        }
        

