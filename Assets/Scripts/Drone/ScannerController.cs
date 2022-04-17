using UnityEngine;

public class ScannerController : MonoBehaviour
{
    [SerializeField]
    Transform scanOrigin;

    bool loadingMainMenu = false;

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
            //print("Collider: " + other.name);
            RaycastHit hit;
            Physics.Linecast(scanOrigin.position, other.transform.position, out hit);
            if (hit.collider.name == "Player" && !loadingMainMenu)
            {
                loadingMainMenu = true;
                GameObject.FindGameObjectWithTag("WinLossUI").GetComponent<WinLossScreenManager>().ActivateLossScreen();
            }
        }
        
    }
}
