using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public enum Direction {
        Up, Left, Down, Right
    }

    public string buildingName;
    public GameObject buildingPrefab;
    public int width;
    public int height;

    //Takes the building origin grid indices and the direction the building is facing to return
    // all the indices in the grid that the building is occupying
    public List<Vector3> GetGridPositionList(Vector3 buildingOrigin, Direction direction) {
        List<Vector3> gridCellsList = new List<Vector3>();

        switch (direction) {
            case Direction.Up:
            case Direction.Down:
                for(int x = 0; x < width; x++) {
                    for(int z = 0; z < height; z++) {
                        gridCellsList.Add(buildingOrigin + new Vector3(x, 0, z));
                    }
                }
                break;
            case Direction.Left:
            case Direction.Right:
                for (int x = 0; x < height; x++) {
                    for (int z = 0; z < width; z++) {
                        gridCellsList.Add(buildingOrigin + new Vector3(x, 0, z));
                    }
                }
                break;

        }

        return gridCellsList;
    }

    //Returns the next direction clockwise when changing direction that the building
    //is facing
    public static Direction GetNextDirection(Direction currentDir) {
        switch (currentDir) {
            default:
            case Direction.Up:
                return Direction.Left;
            case Direction.Left:
                return Direction.Down;
            case Direction.Down:
                return Direction.Right;
            case Direction.Right:
                return Direction.Up;
        }
    }

    //Returns an angle in degrees according to a certain rotation
    //NOTE: Building models should by default be facing downwards, therefore 0 degrees
    public static float GetDirectionAngle(Direction currentDir) {
        switch (currentDir) {
            default:
            case Direction.Up:
                return 180f;
            case Direction.Left:
                return 90f;
            case Direction.Down:
                return 0f;
            case Direction.Right:
                return 270f;
        }
    }

    //Returns a translation needed for the anchor depending on the angle that we want to
    //build the building. This is used due to how rotating a building around the anchor moves
    //the model to an adjacent cell
    public Vector3 GetAnglePosOffset(Direction currentDir) {
        switch (currentDir) {
            default:
            case Direction.Up:
                return new Vector3(width, 0, height);
            case Direction.Left:
                return new Vector3(0, 0, width);
            case Direction.Down:
                return new Vector3(0,0,0);
            case Direction.Right:
                return new Vector3(height, 0, 0);
        }
    }
}
