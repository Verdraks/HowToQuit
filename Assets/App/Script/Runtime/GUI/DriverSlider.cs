using UnityEngine;
using UnityEngine.UI;

public class DriverSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;

    [Space(10)] 
    [SerializeField] private RSO_SliderValue rsoSliderValue;

    private void OnEnable() => rsoSliderValue.OnChanged += UpdateSlider;
    private void OnDisable() => rsoSliderValue.OnChanged -= UpdateSlider;

    private void UpdateSlider()
    {
        slider.value = rsoSliderValue.Value;
    }
    
}