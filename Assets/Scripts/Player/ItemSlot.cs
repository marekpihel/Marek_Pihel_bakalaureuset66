using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    GameObject itemInHand;


    public GameObject GetItemInHand() {
        return itemInHand;
    }

    public bool HasItemInHand() {
        return itemInHand != null;
    }

    public void SetItemInHand(GameObject newItem) {
        if (!HasItemInHand()) {
            this.itemInHand = newItem;     
        }
    }


}
