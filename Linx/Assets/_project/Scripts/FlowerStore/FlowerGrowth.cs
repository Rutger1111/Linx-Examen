using UnityEngine;

public class FlowerGrowth : MonoBehaviour
{
    [SerializeField] private Vector3 GrowthHeight;
    public float _waterLevel;
    private bool _isFullyGrown;

    public Transform Sprout;
    public Transform FullFlower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sprout = gameObject.transform.Find("Sprout");
        FullFlower = gameObject.transform.Find("FullFlower");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale.y >= GrowthHeight.y)
        {
            _isFullyGrown = true;
        }

        if (!_isFullyGrown && _waterLevel > 0)
        {
            _waterLevel -= Time.deltaTime;
            gameObject.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f) * Time.deltaTime;
        }

        if (_isFullyGrown)
        {
            FullGrown();
        }
    }

    private void FullGrown()
    {
        FullFlower.localScale = GrowthHeight;
        Sprout.localScale = new Vector3(0, 0, 0);
    }
}
