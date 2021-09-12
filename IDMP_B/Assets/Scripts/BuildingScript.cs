using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    private BuildingSO buildingTypeSO;
    private Vector3Int originPos;
    private BuildingSO.Direction currentDir;

    [SerializeField]
    private int output;

    public void InitValues(BuildingSO buildingSO, Vector3Int origin, BuildingSO.Direction dir) {
        buildingTypeSO = buildingSO;
        originPos = origin;
        currentDir = dir;
    }

    public void DestroyThis() {
        Debug.Log("DESTROYED");

        Destroy(gameObject);
    }

    public List<Vector3> GetGridPositionList() {
        return buildingTypeSO.GetGridPositionList(originPos, currentDir);
    }

}


