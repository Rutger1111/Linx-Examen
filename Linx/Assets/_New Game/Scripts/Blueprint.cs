using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] private GameObject House;

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
