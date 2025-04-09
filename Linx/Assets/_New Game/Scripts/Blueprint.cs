using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] private GameObject House;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            House.SetActive(true);
        }
        else
        {
            House.SetActive(false);
        }
    }
}
