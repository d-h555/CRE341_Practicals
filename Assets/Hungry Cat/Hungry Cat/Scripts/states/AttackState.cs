using UnityEngine;

public class AttackState : State
{
   public override State RunCurrentState()
   {
    Debug.Log("I have Attacked");
    return this;
   }
}
