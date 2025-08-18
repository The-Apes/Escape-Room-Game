using UnityEngine;
using UnityEngine.AI;

namespace Npc.State_Machine
{
    public class RoamState : IState
    {
        private readonly float _roamRadius = 10f;
        private readonly float _waitTime = 2f;

        private readonly NpcAgent _npcAgent;
        private float _waitTimer;
        private readonly Transform _npcTransform;

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
                if (_waitTimer >= _waitTime)
                {
                    GoToRandomPoint();
                    _waitTimer = 0f;
                }
            }
            return null;
        }
        void GoToRandomPoint()
        {
            //Freaky stuff right here.
            Vector3 randomDirection = Random.insideUnitSphere * _roamRadius;
            randomDirection += _npcTransform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _roamRadius, NavMesh.AllAreas))
            {
                _npcAgent.Agent.SetDestination(hit.position);
            }
        }
    }
}
