using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemManagementSystem : MonoBehaviour
{
    [SerializeField]
    Transform handLocation;
    ItemSlot handSlot;
    PlayerInputActions inputActions;
    

    private void Awake()
    {
        handSlot = new ItemSlot();
        inputActions = new PlayerInputActions();

        inputActions.Player.Fire.started += onThrowingInitiated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bottle"){
            if (!other.GetComponent<BottleController>().HasBeenThrow()) {
                PickupItem(other.gameObject);
            }
        }
    }

    void onThrowingInitiated(InputAction.CallbackContext context)
    {
        if (!GameManager.menuOpened) {
            ThrowItem();
        }
    }

    private void PickupItem(GameObject gameObject)
    {
        handSlot.SetItemInHand(gameObject);
        handSlot.GetItemInHand().GetComponent<BoxCollider>().size = new Vector3(0.23f, 1, 0.23f);
        //handSlot.GetItemInHand().GetComponent<BoxCollider>().isTrigger = false;
        handSlot.GetItemInHand().GetComponent<Rigidbody>().isKinematic = true;
        SetItemLocation(handLocation);
    }

    public void SetItemLocation(Transform handLocation)
    {
        handSlot.GetItemInHand().transform.SetPositionAndRotation(handLocation.position, handLocation.rotation);
        handSlot.GetItemInHand().transform.parent = handLocation;
    }

    private void ThrowItem()
    {
        if (handSlot.HasItemInHand())
        {
            handSlot.GetItemInHand().GetComponent<BottleController>().Throw();
            handSlot.RemoveItemFromHand();
        }
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
