using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowHints : MonoBehaviour
{
    [SerializeField] private GameObject buildingBlock;
    [SerializeField] private List<GameObject> buildingBlocks = new List<GameObject>();

    [SerializeField] private float hintDistance;
    //[SerializeField] private GameObject hintPanel;
    [SerializeField] private Vector3 distance;
    private Camera _camera;
    

    private void Start()
    {
        _camera = Camera.main;
        GameObject[] foundBlocks = GameObject.FindGameObjectsWithTag("BuildingBlock");
        foreach (GameObject block in foundBlocks)
        {
            buildingBlocks.Add(block);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowHint();
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        //hintPanel.transform.LookAt(_camera.transform);
        foreach (GameObject block in buildingBlocks)
        {
            block.transform.GetChild(0).LookAt(_camera.transform);
        }
    }

    private void ShowHint()
    {
        foreach (GameObject block in buildingBlocks)
        {
            Transform blockTransform = block.transform;
            distance = transform.position - blockTransform.position;

            if (Mathf.Abs(distance.x) <= hintDistance &&
                Mathf.Abs(distance.y) <= hintDistance &&
                Mathf.Abs(distance.z) <= hintDistance)
            {
                block.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                block.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
