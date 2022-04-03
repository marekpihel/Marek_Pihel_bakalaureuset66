using UnityEngine;

public class ScannerController : MonoBehaviour
{
    [SerializeField]
    Transform scanOrigin;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            RaycastHit hit;
            Physics.Linecast(scanOrigin.position, other.transform.position, out hit);
            if (hit.collider.name == "Player") {
                FindObjectOfType<GameManager>().LoadMainMenu();
            }
        }
    }
}
