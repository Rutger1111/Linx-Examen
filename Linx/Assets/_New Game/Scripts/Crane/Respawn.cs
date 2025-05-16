using UnityEngine;

namespace _New_Game.Scripts.Crane
{
    public class Respawn : MonoBehaviour
    {
        private Transform _lowestFallPoint;
        private Transform _respawnPoint;

        private void Start()
        {
            _respawnPoint = GameObject.FindWithTag("Respawn").transform;
            _lowestFallPoint = GameObject.FindWithTag("LowestFallPoint").transform;
        }

        private void Update()
        {
            if (transform.position.y <= _lowestFallPoint.transform.position.y)
            {
                transform.position = _respawnPoint.position;
                transform.rotation = _respawnPoint.rotation;
            }
        }
    }
}
