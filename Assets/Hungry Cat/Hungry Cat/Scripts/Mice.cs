using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
public class Mice : MonoBehaviour
{
    public static Mice Instance { get; private set; }
    public float maxSpeed;
    public float maxSight;
    public GameObject player;
   
    NavMeshAgent agent;

public float wanderSpeed;
 public float wanderTime;
private float timeToChangeDirection = 0;
 public AnimationCurve fleeDistanceCurve;

private Vector3 randomDirection;

private Transform playerPosition;

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null)
        {
            playerPosition = player.transform;
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position ) < Vector3.Distance(transform.position, playerPosition.position))
            {
                playerPosition = player.transform;
            }
        }

        if (playerPosition != null)
        {
            Vector3 direction = transform.position - playerPosition.position;
            Vector3 distance = transform.position - playerPosition.position;

            if (distance.magnitude < maxSight)
            {
                Debug.Log("i can see player");
                float distScaled = distance.magnitude/maxSight;
                float fleeDistance =   fleeDistanceCurve.Evaluate(distScaled);
                
                transform.position += direction.normalized * fleeDistance * fleeDistance * maxSpeed * Time.deltaTime;
            }
            else
            {
                if (timeToChangeDirection <= 0)
                {
                    float z = UnityEngine.Random.Range(-1.0f, 1.0f);
                    float x = UnityEngine.Random.Range(-1.0f, 1.0f);
                    randomDirection = new Vector3(x, 0, z);
                    timeToChangeDirection = wanderTime;
                }

                }
            }
        }
    }


