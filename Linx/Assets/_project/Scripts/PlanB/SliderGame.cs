using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace _project.Scripts.PlanB
{
    public class SliderGame : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _sliderValueSetter;

        private bool _sliderOn = true;
        private int _sliderDirection = 1;

        private void Start()
        {
            _sliderValueSetter.value = Random.Range(0, 100);
        }

        private void Update()
        {
            HandleInput();
            UpdateSlider();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckSliderMatch();
            }
            
            //only for testing purposes |
            //                          v
            if (Input.GetKeyDown(KeyCode.E))
            {
                _sliderOn = true;
            }
        }

        private void CheckSliderMatch()
        {
            _sliderOn = false;
            float sliderValue = _slider.value;

            if (Mathf.Abs(sliderValue - _sliderValueSetter.value) <= 20f)
            {
                Debug.Log("success");
                // Add success logic here
            }
        }

        private void UpdateSlider()
        {
            if (!_sliderOn) return;

            _slider.value += _sliderDirection;

            if (_slider.value >= 100 || _slider.value <= 0)
            {
                _sliderDirection *= -1;
            }
        }
    }
}
