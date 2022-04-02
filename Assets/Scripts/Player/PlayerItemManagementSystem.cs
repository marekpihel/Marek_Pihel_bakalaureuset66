using UnityEngine;

public class PlayerItemManagementSystem : MonoBehaviour
{
    [SerializeField]
    Transform handLocation;
    Slot handSlot;
    

    private void Awake()
    {
        handSlot = new Slot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bottle") {
            PickupItem(other.gameObject);
        }
    }

    private void PickupItem(GameObject gameObject)
    {
        handSlot.SetItemInHand(gameObject);
        handSlot.GetItemInHand().GetComponent<BoxCollider>().size = new Vector3(0.23f, 1, 0.23f);
        SetItemLocation(handLocation);
    }

    public void SetItemLocation(Transform handLocation)
    {
        handSlot.GetItemInHand().transform.SetPositionAndRotation(handLocation.position, handLocation.rotation);
        handSlot.GetItemInHand().transform.parent = handLocation;
    }
}
