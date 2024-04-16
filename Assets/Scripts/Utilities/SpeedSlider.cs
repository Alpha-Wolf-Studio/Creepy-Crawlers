using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] private TMP_Text text = null;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        text.text = ChangeSliderValue(slider.value).ToString();
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
    private float ChangeSliderValue(float value)
    {
        Time.timeScale = value;
        return value;
    }
    private void OnSliderValueChanged(float value)
    {
        text.text = ChangeSliderValue(value).ToString();
    }
}