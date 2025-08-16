using UnityEngine;
using UnityEngine.AI;

public class RoamState : State
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

    public State StateUpdate()
    {
        if (!_npcAgent.agent.pathPending && _npcAgent.agent.remainingDistance <= _npcAgent.agent.stoppingDistance)
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
            _npcAgent.agent.SetDestination(hit.position);
        }
    }
}
