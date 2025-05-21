using TMPro;
using UnityEngine;

namespace _New_Game.Scripts
{
    public class PlayedTime : MonoBehaviour
    {
        public float timer;

        public TMP_Text text;

        private void Update()
        {
            timer += Time.deltaTime;
        
            text.text = "time : " + timer.ToString("F2");
        }
    }
}
