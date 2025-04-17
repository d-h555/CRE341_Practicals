using UnityEngine;

namespace Mouse
{
    [CreateAssetMenu(menuName = "A.I/Mouse Actions/Attack Action")]
    // This class is responsible for handling the mouse attack action.
    // It inherits from MonoBehaviour, which allows it to be attached to a GameObject in Unity.
    // The class currently does not contain any functionality, but it can be extended in the future.
    // The Start and Update methods are provided for initialization and frame updates respectively.
    public class MouseAttackAction : MouseActions
    {
        public  int attackScore = 3;
        public float recoveryTime = 2;

        public float maxAttackAngle = 35;
        public float minAttackAngle = 35;
        public float minDistanceNeededToAttack = 0;
        public float maxDistanceNeededToAttack = 3;

    }
}