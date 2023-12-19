using UnityEngine;
using UnityEngine.UI;

namespace Packages.Procedural_Landscape_2D.Code.Extra.LandscapeSettingsGameView
{
    [RequireComponent(typeof(Slider))]
    public class SliderView : MonoBehaviour
    {
         
        [SerializeField] private SliderType sliderType;
        [SerializeField] private LandscapeSettingsView settingsView;
        [SerializeField] private string layerName;
        [SerializeField] private float value;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(OnValueChanged);
        }
        
        private void Start()
        {
            _slider.value = value;
            _slider.maxValue = value * 10f;
            _slider.minValue = value / 10f;
        }

        private void OnValueChanged(float newValue)
        {
            settingsView.ApplyValue(sliderType, newValue, layerName);
        }

        public enum SliderType
        {
            Scale,
            Power
        }
    }
}