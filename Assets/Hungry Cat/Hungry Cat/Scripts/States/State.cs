using Mouse;
using UnityEngine;

public abstract class State : MonoBehaviour

{
    public abstract State RunCurrentState();
    public abstract State Tick(MouseManager mouseManager, EnemyStats enemyStats, AnimatorManager animatorHandler);
}