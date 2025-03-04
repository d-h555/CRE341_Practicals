using Unity.Mathematics;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float maxSpeed;
    public float maxSight;

    public float wanderSpeed;
    public float wanderTime;
    private float timeToChangeDirection = 0;
    private Vector3 randomDirection;
    private Transform playerPosition = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // iterate through all cats in CatMiceGameManager.Instance.catList
        // find the closest cat and move directly away from it 

        foreach (GameObject player in MiceAndCatGameManager .Instance.catList)
        {
            // debug/log 
            // Debug.Log(cat.transform.position);
            
            if (playerPosition == null)
            {
                    playerPosition = player.transform;
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, playerPosition.position))
                {
                    playerPosition=   player.transform;
                }
            }
        }

        if (playerPosition != null)
        {
            Vector3 direction = transform.position - playerPosition.position;
            Vector3 distance = transform.position - playerPosition.position;

            // Debug.Log("cat at this distance " + distance.magnitude);

            if (distance.magnitude < maxSight)
            {
                Debug.Log("player within sight");
                // normalise distance over maxSight
                float distScaled = distance.magnitude/maxSight; 
                // get flee distance value from CatMiceGameManager.Instance.fleeDistanceCurve
                float fleeDistance = MiceAndCatGameManager.Instance.fleeDistanceCurve.Evaluate(distScaled);

                                transform.position += direction.normalized * fleeDistance * maxSpeed * Time.deltaTime;
                            }
                            else
                            {
                                // move randomly
                                // move randomly
                                if (timeToChangeDirection <= 0)
                                {
                                    float x = UnityEngine.Random.Range(-1.0f, 1.0f);
                                    float z = UnityEngine.Random.Range(-1.0f, 1.0f);
                                    randomDirection = new Vector3(x, 0, z);
                                    timeToChangeDirection = wanderTime;
                                }
                                transform.position += randomDirection.normalized * wanderSpeed * Time.deltaTime;
                                timeToChangeDirection -= Time.deltaTime;
                            }
                            
                        }
                    }

                }
