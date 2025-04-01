using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

namespace _project.Scripts.PlanB
{
    public class SliderGame : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _sliderValueSetter;
        [SerializeField] private GameObject canvas;

        [SerializeField] private FishList _fishList;
        
        

        private bool _sliderOn = true;
        private int _sliderDirection = 1;

        private bool SpaceWorks = false;

        private void Update()
        {
            UpdateSlider();
            HandleInput();
        }

        public void StartMiniGame()
        {
            canvas.SetActive(true);
            
            _sliderValueSetter.value = Random.Range(0, 100);

            SpaceWorks = true;
        }
        
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) && SpaceWorks == true)
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
                
                _fishList.CaughtFish();
                
                ResetMiniGame();
            }
            else
            {
                Debug.Log("fail");

                _fishList.FailedFish();
                
                ResetMiniGame();
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

        private void ResetMiniGame()
        {
            _sliderOn = true;
            SpaceWorks = false;
            
            canvas.SetActive(false);
            
            
        }
    }
}
