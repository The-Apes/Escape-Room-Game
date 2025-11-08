using Player;
using UnityEngine;

namespace Sound
{
    public class AmbientSound : MonoBehaviour
    {
        public Collider area;
        private GameObject _player;

        private void Awake()
        {
            _player = FindFirstObjectByType<FpController>().gameObject;
        }

        private void Update()
        {
            Vector3 closestPoint = area.ClosestPoint(_player.transform.position);
            transform.position = closestPoint;
        }
    }
}
