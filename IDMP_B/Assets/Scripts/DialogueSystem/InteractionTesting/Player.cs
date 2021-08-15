using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MoveSpeed = 10f;

    private Rigidbody playerRigidBody;

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI GetDialogueUI()
    {
        return dialogueUI;
    }

    public InteractableInterface Interactable
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen)
        {
            return;
        }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        playerRigidBody.MovePosition(playerRigidBody.position + input.normalized * (MoveSpeed * Time.fixedDeltaTime));

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
    }
}
