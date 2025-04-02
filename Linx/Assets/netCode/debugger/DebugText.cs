using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    public GameObject textPrefab;
    public GameObject content;
    private int addedTexts;
    private DebugTextCollector textCollector;
    // Start is called before the first frame update
    void Start()
    {
        textCollector = DebugTextCollector.GetTextCollector();

    }

    // Update is called once per frame
    void Update()
    {
        if(textCollector.texts.Count >= 1){
            print(textCollector.texts.Count);
            GameObject textObject = Instantiate<GameObject>(textPrefab, content.transform);
            textObject.GetComponent<TMPro.TMP_Text>().text = textCollector.texts[0];
            textCollector.texts.Remove(textCollector.texts[0]);
            textObject.transform.position = new Vector3(content.transform.position.x + 100,content.transform.position.y - addedTexts * 30,0);
            addedTexts ++;
        }
    }
}
