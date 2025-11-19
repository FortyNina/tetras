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

        [SerializeField] private Button _leavesButton;
        [SerializeField] private GameObject _leavesLabel;

        [SerializeField] private Button _bakingSodaButton;
        [SerializeField] private GameObject _bakingSodaLabel;

        private readonly string phTitle = "<size=12>p</size>H {0}";
        private readonly string temperatureTitle = "{0}°F";

        private void Start()
        {
            _temperatureButton.onClick.AddListener(TemperatureButtonClicked);
            _temperatureSlider.onValueChanged.AddListener(TemperatureSliderUpdated);

            _bakingSodaButton.onClick.AddListener(BakingSodaButtonClicked);

            _leavesButton.onClick.AddListener(LeavesButtonClicked);
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

        private void LeavesButtonClicked()
        {
            if (_leavesLabel.activeInHierarchy)
            {
                _leavesLabel.SetActive(false);
                InteractionManager.Instance.EndInteraction();

            }
            else
            {
                _leavesLabel.SetActive(true);
                InteractionManager.Instance.BeginLeavesInteraction();
            }
            _bakingSodaLabel.SetActive(false);

        }

        private void BakingSodaButtonClicked()
        {
            if (_bakingSodaLabel.activeInHierarchy)
            {
                _bakingSodaLabel.SetActive(false);
                InteractionManager.Instance.EndInteraction();

            }
            else
            {
                _bakingSodaLabel.SetActive(true);
                InteractionManager.Instance.BeginBakingSodaInteraction();

            }
            _leavesLabel.SetActive(false);

        }
    }
}