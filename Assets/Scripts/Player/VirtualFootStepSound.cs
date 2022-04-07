using UnityEngine;

public class VirtualFootStepSound : MonoBehaviour
{
    BoxCollider footstepRange;
    int height = 3;
    Vector3 walkingFootstepRange = new Vector3(10, 0, 10);

    private void Awake()
    {
        footstepRange = GetComponent<BoxCollider>();
    }

    internal void ChangeFootstepHeardRange(float modifier)
    {
        footstepRange.size = walkingFootstepRange * modifier + Vector3.up * height;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Drone") {
            other.GetComponent<DroneBehaviourController>().InvestigatePoint(this.transform.position);
        }
    }

}
