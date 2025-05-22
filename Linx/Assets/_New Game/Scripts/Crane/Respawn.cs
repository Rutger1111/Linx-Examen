using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _New_Game.Scripts.Crane
{
    public class Respawn : MonoBehaviour
    {
        private float timer = 0.1f;
        
        private Transform _lowestFallPoint;
        private Transform _respawnPoint, _respawnPointWall;

        private Transform _north, _south, _east, _west;

        private Rigidbody rb;
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
                rb = GetComponent<Rigidbody>();
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
                if (this.gameObject.tag == "Wall")
                {
                    RespawnPointWall();
                    return;
                }
                
                RespawnPoint();
            }

            if (transform.position.x >= _north.transform.position.x || transform.position.x <= _south.transform.position.x)
            {
                if (this.gameObject.tag == "Wall")
                {
                    RespawnPointWall();
                    return;
                }
                
                RespawnPoint();
            }
            
            if (transform.position.z >= _west.transform.position.z || transform.position.z <= _east.transform.position.z)
            {
                if (this.gameObject.tag == "Wall")
                {
                    RespawnPointWall();
                    return;
                }
                
                RespawnPoint();
            }

            timer -= Time.deltaTime;
            
            if (rb != null && timer <= 0)
            {
                rb.isKinematic = false;
            }
        }

        private void RespawnPointWall()
        {
            
            transform.position = _respawnPointWall.position;
            transform.rotation = _respawnPointWall.rotation;
            
           

            if (rb != null)
            {
                timer = 0.1f;
            
                rb.isKinematic = true;    
            }
        }

        private void RespawnPoint()
        {
            transform.position = _respawnPoint.position;
            transform.rotation = _respawnPoint.rotation;

        }
    }
}
