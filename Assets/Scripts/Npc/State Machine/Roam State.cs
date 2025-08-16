using Npc.State_Machine;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : IState
{
    public float roamRadius = 10f;
    public float waitTime = 2f;

    private NpcAgent _npcAgent;
    private float _waitTimer;
    private Transform _npcTransform;

    public RoamState(NpcAgent npcAgent, Transform npcTransform)
    {
        _npcAgent = npcAgent;
        _npcTransform = npcTransform;
        GoToRandomPoint();
    }

    public IState StateUpdate()
    {
        if (!_npcAgent.Agent.pathPending && _npcAgent.Agent.remainingDistance <= _npcAgent.Agent.stoppingDistance)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                GoToRandomPoint();
                _waitTimer = 0f;
            }
        }
        return null;
    }
    void GoToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += _npcTransform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            _npcAgent.Agent.SetDestination(hit.position);
        }
    }
}
