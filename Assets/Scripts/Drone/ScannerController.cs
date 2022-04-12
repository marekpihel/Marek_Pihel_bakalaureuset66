using UnityEngine;

public class ScannerController : MonoBehaviour
{
    [SerializeField]
    Transform scanOrigin;
    private void OnTriggerStay(Collider other)
    {
        CheckForPlayer(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForPlayer(other);
    }

    private void CheckForPlayer(Collider other)
    {
        if (other.tag == "Player")
        {
            RaycastHit hit;
            Physics.Linecast(scanOrigin.position, other.transform.position, out hit);
            if (hit.collider.name == "Player")
            {
                FindObjectOfType<GameManager>().LoadMainMenu();
            }
            else
            {
                print(hit.collider.name);
            } 
        }
        
    }
}
