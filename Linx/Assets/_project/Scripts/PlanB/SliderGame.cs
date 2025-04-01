using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

namespace _project.Scripts.PlanB
{
    public class SliderGame : MonoBehaviour
    {
        private float _setSliderValue;
        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _image;

        private bool _sliderFull;

        private bool _sliderOn;
        
        void Start()
        {
            _sliderOn = true;
            
        }

        
        void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _image.value = Random.Range(0, 100);
                _setSliderValue = _slider.value;
                _sliderOn = false;
                Debug.Log(_setSliderValue);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _sliderOn = true;
            }
            
            if (_slider.value >= 100)
            {
                _sliderFull = true;
            }
            else if (_slider.value <= 0)
            {
                _sliderFull = false;
            }
            if (!_sliderFull && _sliderOn)
            {
                _slider.value++;
            }

            if (_sliderFull && _sliderOn)
            {
                _slider.value--;
            }
        }
    }
}
