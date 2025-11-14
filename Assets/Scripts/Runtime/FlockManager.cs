using System.Collections.Generic;
using UnityEngine;

namespace Tetras
{
    /// <summary>
    /// Handles instantiating and steering a flock
    /// </summary>
    public class FlockManager : MonoBehaviour
    {
        public static FlockManager Instance;

        [Header("References")]
        [SerializeField] private Boid _boidPrefab;

        [Space(20)]
        [Header("Flock Settings")]
        [SerializeField] private int _flockCount = 250;
        [SerializeField, Range(1f, 100f)] private float _maxSpeed = 5f;
        [SerializeField, Range(1f, 10f)] private float _neighborDistance = 2f;
        [SerializeField, Range(0f, 1)] private float _avoidanceRadiusFactor = .5f;
        [SerializeField] private float _flockRadius = 10;
        [SerializeField, Range(0f, 1f)] private float _edgeCutoff = .9f;

        [Space(20)]
        [Header("Behavior Strengths")]
        [SerializeField] private float _alignmentStrength = 1f;
        [SerializeField] private float _cohesionStrength = 4f;
        [SerializeField] private float _separationStrength = 2f;
        [SerializeField] private float _edgesStrength = .5f;
        [SerializeField] private float _avoidanceStrength = 3f;

        private List<Boid> _boids = new List<Boid>();
        public int BoidCount => _boids.Count;

        private Vector3 _currentVelocity;
        private float _boidSmoothTime = .5f;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
        }


        private void Start()
        {
            for (int i = 0; i < _flockCount; i++)
            {
                Boid newBoid = Instantiate(_boidPrefab, Random.insideUnitSphere * 8f, Quaternion.Euler(Vector3.forward * Random.Range(0, 360f)), transform);
                _boids.Add(newBoid);
            }
        }


        private void Update()
        {
            for (int i = 0; i < _boids.Count; i++)
            {
                Boid boid = _boids[i];

                List<Transform> boids = boid.GetNearbyColliders(_neighborDistance);

                Vector3 move = Steer(boid, boids);

                move *= 100;
                if (move.sqrMagnitude > Mathf.Pow(_maxSpeed,2f)) move = move.normalized * _maxSpeed; // reset to max speed

                boid.Move(move);
            }
        }

        #region Flocking Operations

        private Vector3 Steer(Boid boid, List<Transform> boids)
        {
            Vector3 steering = Vector3.zero;

            steering += Calculate(Align(boid, boids), _alignmentStrength);
            steering += Calculate(Cohesion(boid, boids), _cohesionStrength);
            steering += Calculate(Separation(boid, boids), _separationStrength);
            steering += Calculate(AvoidEdges(boid), _edgesStrength);

            return steering;
        }

        private Vector3 Calculate(Vector3 steering, float strength)
        {
            Vector3 newSteering = steering;
            if (newSteering != Vector3.zero && newSteering.sqrMagnitude > Mathf.Pow(strength, 2f))
            {
                newSteering.Normalize();
                newSteering *= strength;
            }
            return newSteering;
        }

        /// <summary>
        /// Returns the average forward vector of all boids. Used to keep a boid moving in same direction as rest of flock
        /// </summary>
        private Vector3 Align(Boid boid, List<Transform> boids)
        {
            if (boids.Count == 0) return boid.transform.forward;
            Vector3 steering = Vector3.zero;

            for (int i = 0; i < boids.Count; i++) steering += boids[i].transform.forward;

            steering = steering / boids.Count;
            return steering;
        }

        /// <summary>
        /// Returns a Vector towards the center of mass of the flock, from the current position of the given boid. Used to keep boids in the flock
        /// </summary>
        private Vector3 Cohesion(Boid boid, List<Transform> boids)
        {
            if (boids.Count == 0) return Vector3.zero;
            Vector3 steering = Vector3.zero;

            for (int i = 0; i < boids.Count; i++) steering += boids[i].position;

            steering = steering / boids.Count;
            steering = steering -= boid.transform.position;
            steering = Vector3.SmoothDamp(boid.transform.forward, steering, ref _currentVelocity, _boidSmoothTime);

            return steering;
        }

        /// <summary>
        /// Prevents flock from converging to one point
        /// </summary>
        private Vector3 Separation(Boid boid, List<Transform> boids)
        {
            if (boids.Count == 0) return Vector3.zero;

            Vector3 steering = Vector3.zero;
            int avoidCount = 0;
            var avoidanceFactor = Mathf.Pow(_avoidanceRadiusFactor, 2f) * Mathf.Pow(_neighborDistance, 2f);
            for (int i = 0; i < boids.Count; i++)
            {
                if (Vector3.SqrMagnitude(boids[i].position - boid.transform.position) < avoidanceFactor)
                {
                    avoidCount++;
                    steering += (boid.transform.position - boids[i].position);
                }
            }
            if (avoidCount > 0) steering = steering / avoidCount;
            return steering;
        }

        private Vector3 AvoidEdges(Boid boid)
        {
            Vector3 centerOffset = Vector3.zero - boid.transform.position;
            float t = centerOffset.magnitude / _flockRadius;
            if (t < .9f) return Vector3.zero;
            return centerOffset * t * t;
        }

        #endregion
    }
}