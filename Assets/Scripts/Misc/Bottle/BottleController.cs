using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField]
    SphereCollider soundCollider;
    [SerializeField]
    GameObject bottleBreakingSound;

    Rigidbody rigibody;
    bool isThrown = false, expandingArea = false;
    private float expandingSpeed = 100f;
    private float maxExpansionArea = 20f;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        isThrown = true;
        transform.parent = null;
        rigibody.isKinematic = false;
        rigibody.AddRelativeForce(Vector3.back * 10000 + Vector3.down * 5000);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Environment" && isThrown)
        {
            soundCollider.enabled = true;
            PlaySound(transform);
            SimulateSound();
        }
        else if (collider.tag == "Drone")
        {
            expandingArea = false;
            NotifyClosestEnemy(collider);
            BreakBottle();

        }
    }

    private void SimulateSound()
    {
        expandingArea = true;
    }

    private void PlaySound(Transform location)
    {
        Instantiate(bottleBreakingSound, location.position, location.rotation);
    }

    private void NotifyClosestEnemy(Collider droneCollider)
    {
        DroneBehaviourController droneController = droneCollider.GetComponent<DroneBehaviourController>();
        droneController.InvestigatePoint(this.gameObject.transform.position);
    }

    private void BreakBottle()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (expandingArea)
        {
            soundCollider.radius += expandingSpeed * Time.deltaTime;
            if (soundCollider.radius >= maxExpansionArea)
            {
                BreakBottle();
            }
        }
    }

    public bool HasBeenThrow()
    {
        return isThrown;
    }
}
