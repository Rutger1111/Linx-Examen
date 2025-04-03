using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

namespace _project.Scripts.PlanB
{
    public class SliderGame : NetworkBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _sliderValueSetter;
        [SerializeField] private GameObject canvas;

        [SerializeField] private FishingManager fishingManager;
        
        private int answersReceived = 0;
        private bool player1Correct = false;
        private bool player2Correct = false;

        private bool _sliderOn = true;
        private int _sliderDirection = 1;

        private bool SpaceWorks = false;
        
        public float speed = 10f;
        
        private readonly Dictionary<ulong, bool> playerAnswers = new();

        private void Update()
        {
            UpdateSlider();
            HandleInput();
        }

        public void StartMiniGame()
        {
            ToggleCanvasServerRpc(true);
            
            _sliderValueSetter.value = Random.Range(0, 100);

            SpaceWorks = true;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void ToggleCanvasServerRpc(bool state)
        {
            ToggleCanvasClientRpc(state);
        }

        [ClientRpc]
        void ToggleCanvasClientRpc(bool state)
        {
            canvas.SetActive(state);
        }
        
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("go though");
                
                HandleInputServerRpc();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void HandleInputServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            HandleInputClientRpc(clientId);
        }

        [ClientRpc]
        void HandleInputClientRpc(ulong clientId)
        {
            print(clientId);
            
            if (NetworkManager.LocalClientId == clientId) 
            {
                canvas.SetActive(false); 
            }
            
            print("work");
            SubmitAnswerServerRpc( true,clientId);
        }
        
        [ServerRpc(RequireOwnership = false)]
        void SubmitAnswerServerRpc(bool isCorrect, ulong clientId)
        {
            if (!playerAnswers.ContainsKey(clientId))
            {
                playerAnswers[clientId] = isCorrect;
                answersReceived++;
                
                
            }

            if (answersReceived >= 2) 
            {
                CheckSliderMatch();
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
                
                fishingManager.HandleFishCaughtServerRpc();
                
                ResetMiniGame();
            }
            else
            {
                Debug.Log("fail");

                fishingManager.HandleFishFailedServerRpc();
                
                ResetMiniGame();
            }
        }

        
        
        private void UpdateSlider()
        {
            if (!_sliderOn) return;
            
            _slider.value += _sliderDirection * speed * Time.deltaTime;

            if (_slider.value >= 100 || _slider.value <= 0)
            {
                _sliderDirection *= -1;
            }
        }

        private void ResetMiniGame()
        {
            _sliderOn = true;
            SpaceWorks = false;
            
            answersReceived = 0;
            player1Correct = false;
            player2Correct = false;
            
            canvas.SetActive(false);
            
            
        }
    }
}
