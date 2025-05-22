using UnityEngine;

public class playSong : MonoBehaviour
{
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void buttonClick()
    {
        _audioSource.Play();
    }
}
