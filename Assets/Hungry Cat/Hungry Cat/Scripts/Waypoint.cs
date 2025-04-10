using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using System.Collections.Generic;
public class Waypoint : StateMachineBehaviour
{
    GameObject MOUSE;
    bool isWalking = false;

    // list of gameObject waypoints
    List<GameObject> waypoints;
    [SerializeField] Transform WaypointTarget;
    
void Start(Animator animator)
    {
       Animator animatorComponent = animator;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // debug statement 
        Debug.Log("Entering Patrol State");
        isWalking = true;

        // get all waypoints with tag Waypoint
        waypoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoint"));
        WaypointTarget = waypoints[Random.Range(0, waypoints.Count)].transform;

        MOUSE = GameObject.Find("NPC_00");
        MOUSE.GetComponent<NavMeshAgent>().SetDestination(WaypointTarget.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug log showing the current state
        Debug.Log("On State Update ~ Patrol State");

        // get parent object of the object containing the animator
        if (Vector3.Distance(MOUSE.transform.position, WaypointTarget.position) < 0.1f)
        {
            WaypointTarget = waypoints[Random.Range(0, waypoints.Count)].transform;
            MOUSE.GetComponent<NavMeshAgent>().SetDestination(WaypointTarget.position);
        }
        
        //NPC_00.transform.position = Vector3.MoveTowards(animator.transform.position, WaypointTarget.position, GameManager.Instance.NPC_AI_01.Speed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // debug statement 
        Debug.Log("Exiting Patrol State");
        isWalking = false;
    }

}
