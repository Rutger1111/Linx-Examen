using UnityEngine;

public class GameInput : MonoBehaviour
{
    public GameObject flower;

    public FlowerGrowth _flowerGrowth;

    private bool scriptFound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(flower).transform.position = new Vector3(0, 0.5f, 0);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            _flowerGrowth._waterLevel += 10 * Time.deltaTime;
        }

        
        if (!scriptFound)
        {
            FindScript();
        }
        else if (_flowerGrowth != null)
        {
            scriptFound = true;
        }
    }

    private void FindScript()
    {
        _flowerGrowth = FindFirstObjectByType<FlowerGrowth>();
    }
}
