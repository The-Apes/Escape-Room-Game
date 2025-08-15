using UnityEngine;
using UnityEngine.AI;

public class RoamState : State
{
    public float roamRadius = 10f;
    public float waitTime = 2f;

    private NavMeshAgent _agent;
    private float _waitTimer;
    private Transform _npcTransform;

    public RoamState(NavMeshAgent agent, Transform npcTransform)
    {
        _agent = agent;
        _npcTransform = npcTransform;
        GoToRandomPoint();
    }

    public State StateUpdate()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                GoToRandomPoint();
                _waitTimer = 0f;
            }
        }
        return this;
    }
    void GoToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += _npcTransform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
}
