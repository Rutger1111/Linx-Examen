using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public string[] TextLines;

    public TMP_Text textUITMPText;

    public int index;

    public float textSpeed;

    public bool fullLine;
    void Start()
    {
        
            index = 0;
            fullLine = false;
          

        startText();
    }

    private void Update()
    {
        if (TextLines.Length == index + 1)
        {
            print("No More Lines");
        }
        else
        {
            if (textUITMPText.text == TextLines[index])
            {
                fullLine = true;
                StopAllCoroutines();
                textUITMPText.text = TextLines[index];
            }
            else
            {
                fullLine = false;
            }    
        }
        
    }

    void startText()
    {
        StartCoroutine(TypeLine());
    }
    public IEnumerator TypeLine()
    {
        textUITMPText.text = "";
        
        if (textUITMPText.text != TextLines[index].ToString())
        {
            foreach (char c in TextLines[index].ToCharArray())
            {
                textUITMPText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        
    }

    public void nextText()
    {
        if (fullLine == true)
        {
            index++;
            fullLine = false;
            StartCoroutine(TypeLine());
        }
        else
        {
            StopAllCoroutines();
            textUITMPText.text = TextLines[index];
            
        }
    }
}
