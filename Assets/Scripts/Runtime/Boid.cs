using System.Collections.Generic;
using UnityEngine;

namespace Tetras
{
    public class Boid : MonoBehaviour
    {
        [SerializeField] private Color[] _fishColors;

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
            SetColor();
        }

        private void SetColor()
        {
            var neighborPercent = 1f / (GetNearbyColliders(2f).Count / 6f);
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