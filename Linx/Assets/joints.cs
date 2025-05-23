using UnityEngine;

public class joints : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;

    void Start()
    {
        if (objectA != null && objectB != null)
        {
            Vector3 midpoint = (objectA.transform.position + objectB.transform.position) / 2f;
            Debug.Log("Midpoint: " + midpoint);
        }
        else
        {
            Debug.LogWarning("Please assign both GameObjects.");
        }
    }
}
