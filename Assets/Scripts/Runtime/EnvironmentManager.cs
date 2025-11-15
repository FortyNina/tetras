using UnityEngine;

namespace Tetras
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance;

        [SerializeField] private float _pH = 7.0f;
        public float PH => _pH;

        [SerializeField] private int _temperature = 75;
        public int Temperature => _temperature;

        public System.Action OnPHUpdated;
        public System.Action OnTemperatureUpdated;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
        }

        public void UpdatePH(float newValue)
        {
            _pH = newValue;
            OnPHUpdated?.Invoke();
        }

        public void UpdateTemperature(int newValue)
        {
            _temperature = newValue;
            OnTemperatureUpdated?.Invoke();
        }

    }
}