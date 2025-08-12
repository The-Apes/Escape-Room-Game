using UnityEngine;
using UnityEngine.AI;

public class RoamState : MonoBehaviour, State
{
    private NavMeshAgent _agent;
   // private NavMesh _navMesh;

    public void enterState()
    {
        _agent = FindFirstObjectByType<NavMeshAgent>();
    }

    public void StateUpdate()
    {
       // _agent.Find
      // _navMesh.
    }
}
