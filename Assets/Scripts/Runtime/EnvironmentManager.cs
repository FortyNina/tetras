using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Tetras
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance;

        [SerializeField] private Volume _volume;

        [Space(20), Header("Temperature")]
        [SerializeField] private int _temperature = 75;
        public int Temperature => _temperature;
        [SerializeField, ColorUsage(true, true)] private Color _normalTemperatureColor;
        [SerializeField, ColorUsage(true, true)] private Color _coldTemperatureColor;
        [SerializeField, ColorUsage(true, true)] private Color _warmTemperatureColor;
        [SerializeField] private int _normalTemperature = 75;
        public int NormalTemperature => _normalTemperature;
        [SerializeField] private int _coldTemperature = 45;
        public int ColdTemperature => _coldTemperature;
        [SerializeField] private int _warmTemperature = 100;
        public int WarmTemperature => _warmTemperature;

        [Space(20), Header("pH")]
        [SerializeField] private float _pH = 7.0f;
        public float PH => _pH;


        private Bloom _bloom;

        public System.Action OnPHUpdated;
        public System.Action OnTemperatureUpdated;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;

            _volume.profile.TryGet(out _bloom);
        }

        public void UpdatePH(float newValue)
        {
            _pH = newValue;
            OnPHUpdated?.Invoke();
        }

        public void UpdateTemperature(float newValue)
        {
            if (newValue <= 0)
            {
                _temperature = (int)Mathf.Lerp(_coldTemperature, _normalTemperature, 1 - Mathf.Abs(newValue));
                var color = Color.Lerp(_coldTemperatureColor, _normalTemperatureColor, 1 - Mathf.Abs(newValue));
                _bloom.tint.Override(color);
            }
            else
            {
                _temperature = (int)Mathf.Lerp(_normalTemperature, _warmTemperature, Mathf.Abs(newValue));
                var color = Color.Lerp(_normalTemperatureColor, _warmTemperatureColor, Mathf.Abs(newValue));
                _bloom.tint.Override(color);
            }

            OnTemperatureUpdated?.Invoke();
        }

    }
}