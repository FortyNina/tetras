using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Tetras
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _phText;
        [SerializeField] private TextMeshProUGUI _temperatureText;

        [SerializeField] private Button _temperatureButton;
        [SerializeField] private Slider _temperatureSlider;

        private readonly string phTitle = "<size=12>p</size>H {0}";
        private readonly string temperatureTitle = "{0}°F";

        private void Start()
        {
            _temperatureButton.onClick.AddListener(TemperatureButtonClicked);
            _temperatureSlider.onValueChanged.AddListener(TemperatureSliderUpdated);
        }

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

        private void TemperatureButtonClicked()
        {
            _temperatureSlider.gameObject.SetActive(!_temperatureSlider.gameObject.activeInHierarchy);
        }

        private void TemperatureSliderUpdated(float value)
        {
            EnvironmentManager.Instance.UpdateTemperature(value);
        }
    }
}