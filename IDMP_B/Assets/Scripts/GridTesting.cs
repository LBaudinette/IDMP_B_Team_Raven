using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridTesting : MonoBehaviour
{
    private GridXZ grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GridXZ(10, 5, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 420);
        }
    }
}
