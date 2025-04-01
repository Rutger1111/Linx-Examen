using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject _canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(-255, 300, 0);
        transform.SetParent(_canvas.transform);
        _canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
