using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    LevelManager lm;
    Vector3 pos;
    Vector3 origin;
    public GridBuilder gb;
    float cellSize;

    public bool playerMoving;
    private Animator playerAnimator;
    private Vector3 nodePos;

    //UserInterface
    [SerializeField] private DialogueUI dialogueUI;

    // Start is called before the first frame update
    void Start()
    {
        playerMoving = false;
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetInteger("AnimationPar", 0);
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        origin = gb.transform.position;
        pos = lm.startPos;
        cellSize = gb.GetCellSize();
        UpdateGridPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen)
        {
            return;
        }

        if (playerMoving)
        {
            if (Vector3.Distance(transform.position, (new Vector3(pos.x, 0, pos.z) + origin) * cellSize + new Vector3(cellSize / 2, 0, cellSize / 2)) >= 0.05f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * 2, Space.Self);
            } else
            {
                playerMoving = false;
                UpdateGridPos();
                playerAnimator.speed = 1;
                playerAnimator.SetInteger("AnimationPar", 0);
            }
            return;
        }

        CheckMousePos();
    }

    void CheckMousePos()
    {
        // get mouse position on grid
        // if player inputs left click to move
        if (Input.GetMouseButtonDown(0))
        {
            // get target node pos and node obj
            Vector3 mousePos = gb.GetMouseWorldPos();
            Vector3 nodePos = gb.GetXZCell(mousePos);
            GridObject nodeObj = gb.GetGridObject(mousePos);

            // if obj is not outside grid bounds && can be built on (and thus walked on)
            if (nodeObj != default && !nodeObj.hasPrimary())
            {
                //if grid node at mouse position is adjacent to player's current position
                if (IsPlayerAdjacent(nodePos))
                {
                    playerMoving = true;
                    playerAnimator.speed = 2;
                    playerAnimator.SetInteger("AnimationPar", 1);


                    //update player's position to adjacent grid node
                    pos.x = nodePos.x;
                    pos.z = nodePos.z;
                    //UpdateGridPos();

                    transform.LookAt((new Vector3(pos.x, 0, pos.z) + origin) * cellSize + new Vector3(cellSize / 2, 0, cellSize / 2), Vector3.up);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                    // new action (movement) has been performed, increment action count in level manager
                    lm.OnNewAction();

                }
            }
        }
    }

    void UpdateGridPos()
    {
        // update gameobject transform to (x * cellSize + cellSize / 2, y, z * cellSize + cellSize / 2)]
        this.gameObject.transform.position = (new Vector3(pos.x, 0, pos.z) + origin) * cellSize + new Vector3(cellSize / 2, 0, cellSize / 2);
    }

    public bool IsPlayerAdjacent(Vector3 position)
    {
        float xDiff = pos.x - position.x;
        float zDiff = pos.z - position.z;
        return (Mathf.Abs(xDiff) == 1 && zDiff == 0) || (Mathf.Abs(zDiff) == 1 && xDiff == 0);
    }

    private void RotatePlayer()
    {
        transform.LookAt(nodePos, Vector3.up);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}