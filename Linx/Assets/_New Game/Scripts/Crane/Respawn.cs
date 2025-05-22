using UnityEngine;

namespace _New_Game.Scripts.Crane
{
    public class Respawn : MonoBehaviour
    {
        private float _timer = 0.1f;
        
        private Transform _lowestFallPoint;
        private Transform _respawnPoint, _respawnPointWall;

        private Transform _north, _south, _east, _west;

        private Rigidbody _rb;
        private void Start()
        {
            _respawnPoint = GameObject.FindWithTag("Respawn").transform;
            _respawnPointWall = GameObject.FindWithTag("respawnPointWall").transform;
            
            _lowestFallPoint = GameObject.FindWithTag("LowestFallPoint").transform;
            _north = GameObject.FindWithTag("North").transform;
            _south =  GameObject.FindWithTag("South").transform;
            _east =  GameObject.FindWithTag("East").transform;
            _west =  GameObject.FindWithTag("West").transform;

            try
            {
                _rb = GetComponent<Rigidbody>();
            }
            catch
            {
                throw;
            }
        }

        private void Update()
        {
            if (transform.position.y <= _lowestFallPoint.transform.position.y)
            {
                if (gameObject.CompareTag("Wall"))
                {
                    RespawnPointWall();
                    return;
                }
                
                RespawnPoint();
            }

            if (transform.position.x >= _north.transform.position.x || transform.position.x <= _south.transform.position.x)
            {
                if (gameObject.CompareTag("Wall"))
                {
                    RespawnPointWall();
                    return;
                }
                
                RespawnPoint();
            }
            
            if (transform.position.z >= _west.transform.position.z || transform.position.z <= _east.transform.position.z)
            {
                if (gameObject.CompareTag("Wall"))
                {
                    RespawnPointWall();
                    return;
                }
                
                RespawnPoint();
            }

            _timer -= Time.deltaTime;
            
            if (_rb != null && _timer <= 0)
            {
                _rb.isKinematic = false;
            }
        }

        private void RespawnPointWall()
        {
            
            transform.position = _respawnPointWall.position;
            transform.rotation = _respawnPointWall.rotation;
            
           

            if (_rb != null)
            {
                _timer = 0.1f;
            
                _rb.isKinematic = true;    
            }
        }

        private void RespawnPoint()
        {
            transform.position = _respawnPoint.position;
            transform.rotation = _respawnPoint.rotation;

        }
    }
}
