using UnityEngine;

namespace _New_Game.Scripts
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private GameObject house;

        // Update is called once per frame
        void Update()
        {
            house.SetActive(Input.GetKey(KeyCode.Tab));
        }
    }
}
