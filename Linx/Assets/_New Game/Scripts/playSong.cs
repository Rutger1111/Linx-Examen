using UnityEngine;

namespace _New_Game.Scripts
{
    public class PlaySong : MonoBehaviour
    {
        private AudioSource _audioSource;
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void ButtonClick()
        {
            _audioSource.Play();
        }
    }
}
