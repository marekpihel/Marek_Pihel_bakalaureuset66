using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerController : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            RaycastHit hit;
            Physics.Linecast(transform.position, other.transform.position, out hit);
            if (hit.collider.name == "Player") {
                FindObjectOfType<GameManager>().LoadMainMenu();
            }
        }
    }
}
