using System.Net.Mime;
using TMPro;
using UnityEngine;

public class PlayedTime : MonoBehaviour
{
    public float Timer;

    public TMP_Text text;
    
    void Update()
    {
        Timer += Time.deltaTime;
        
        text.text = "time : " + Timer.ToString("F2");
    }
}
