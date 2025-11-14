using System.Collections.Generic;
using UnityEngine;

namespace Tetras
{
    public class Boid : MonoBehaviour
    {
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }


        public void Move(Vector3 velocity)
        {
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity;
            }
            transform.position += velocity * Time.deltaTime;

        }

        /// <summary>
        /// Returns a list of colliders within range of this boid
        /// </summary>
        public List<Transform> GetNearbyColliders(float neighborDistance)
        {
            List<Transform> objs = new List<Transform>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, neighborDistance);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != _collider) objs.Add(colliders[i].transform);
            }
            return objs;
        }

        
    }
}