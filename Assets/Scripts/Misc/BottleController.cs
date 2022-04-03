using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField]
    SphereCollider soundCollider;

    Rigidbody rigibody;
    bool isThrown = false, expandingArea = false;
    private float expandingSpeed = 100f;
    private float maxExpansionArea = 20f;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    public void Throw() {
        isThrown = true;
        transform.parent = null;
        rigibody.isKinematic = false;
        rigibody.AddRelativeForce(Vector3.back * 500 + Vector3.down * 250);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Environment" && isThrown) {
            soundCollider.enabled = true;
            PlaySound();
            SimulateSound();
        } else if (collider.tag == "Drone") {
            expandingArea = false;
            NotifyClosestEnemy(collider);
            BreakBottle();
        }
    }

    private void SimulateSound()
    {
        expandingArea = true;
    }

    private void PlaySound()
    {
        //ToDo: Implement sound emitter
        print("Playing sound");
    }

    private void NotifyClosestEnemy(Collider droneCollider)
    {
        droneCollider.GetComponent<DroneMovementController>().InvestigatePoint();
    }

    private void BreakBottle()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (expandingArea) {
            soundCollider.radius += expandingSpeed * Time.deltaTime;
            if(soundCollider.radius >= maxExpansionArea){
                BreakBottle();
            }
        }
    }

    public bool HasBeenThrow() {
        return isThrown;
    }
}
