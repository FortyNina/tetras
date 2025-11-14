using System.Collections.Generic;
using UnityEngine;

namespace Tetras
{
    public class Boid : MonoBehaviour
    {
        [SerializeField] private Color[] _fishColors;

        private List<Transform> _nearbyColliders = new List<Transform>();
        public List<Transform> NearbyColliders => _nearbyColliders;

        private float _nearestObstacleDistance;
        public float NearestObstacleDistance => _nearestObstacleDistance;
        private List<Transform> _nearbyObstacles = new List<Transform>();
        public List<Transform> NearbyObstacles => _nearbyObstacles;

        private Collider _collider;
        private ParticleSystemRenderer _particleSystem;
        private Material _particleSystemMat;

        private readonly string _particleMatEmissionKey = "_EmissionColor";
        private readonly string _particleMatBaseKey = "_BaseColor";

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _particleSystem = GetComponentInChildren<ParticleSystemRenderer>();
            _particleSystemMat = _particleSystem.material;
        }

        private void Update()
        {
            UpdateNearbyColliders(FlockManager.Instance.NeighborDistance);
            UpdateNearbyObstacles();
            SetColor();
        }

        private void SetColor()
        {
            var neighborPercent = 1f / (NearbyColliders.Count / 6f);
            float index = neighborPercent * _fishColors.Length;
            int floorIndex = (int)index;
            int ceilingIndex = floorIndex + 1;
            float remainder = index - (float)floorIndex;
            floorIndex = Mathf.Clamp(floorIndex, 0, _fishColors.Length - 1);
            ceilingIndex = Mathf.Clamp(ceilingIndex, 0, _fishColors.Length - 1);
            var col = Color.Lerp(_fishColors[floorIndex], _fishColors[ceilingIndex], remainder);

            _particleSystemMat.SetColor(_particleMatEmissionKey, col);
            _particleSystemMat.SetColor(_particleMatBaseKey, col);

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
        /// Sets a list of colliders within range of this boid.
        /// </summary>
        private void UpdateNearbyColliders(float neighborDistance)
        {
            List<Transform> objs = new List<Transform>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, neighborDistance);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != _collider) objs.Add(colliders[i].transform);
            }
            _nearbyColliders = objs;
        }

        /// <summary>
        /// Updates the list of obstacles within range. Obstacles are objects in the world tagged as obstacle
        /// </summary>
        /// <returns></returns>
        private void UpdateNearbyObstacles()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3);
            List<Transform> obstacles = new List<Transform>();
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag.Equals("Obstacle"))
                {
                    obstacles.Add(hitCollider.transform);
                }
            }

            if (obstacles.Count == 0) _nearestObstacleDistance = 0;
            else
            {
                //find closest
                float minDist = Mathf.Infinity;
                for (int i = 0; i < obstacles.Count; i++)
                {
                    float dist = Vector3.Distance(transform.position, obstacles[i].position);
                    if (dist < minDist) minDist = dist;
                }
                _nearestObstacleDistance = minDist;
            }

            _nearbyObstacles = obstacles;

        }


    }
}