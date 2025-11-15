using UnityEngine;
using TMPro;

namespace Tetras
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _phText;
        [SerializeField] private TextMeshProUGUI _temperatureText;

        private readonly string phTitle = "<size=12>p</size>H {0}";
        private readonly string temperatureTitle = "{0}°F";

        private void OnEnable()
        {
            EnvironmentManager.Instance.OnPHUpdated += UpdateText;
            EnvironmentManager.Instance.OnTemperatureUpdated += UpdateText;

            UpdateText();
        }

        private void OnDisable()
        {
            EnvironmentManager.Instance.OnPHUpdated -= UpdateText;
            EnvironmentManager.Instance.OnTemperatureUpdated -= UpdateText;
        }

        private void UpdateText()
        {
            _phText.text = string.Format(phTitle, EnvironmentManager.Instance.PH.ToString("F1"));
            _temperatureText.text = string.Format(temperatureTitle, EnvironmentManager.Instance.Temperature);
        }
    }
}