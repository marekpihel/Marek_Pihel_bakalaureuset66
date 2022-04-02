using UnityEngine;

public class BottleController : MonoBehaviour
{
    bool hasBeenThrown = false;
    Rigidbody rigibody;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    public void Throw() {
        hasBeenThrown = true;
        transform.parent = null;
        rigibody.isKinematic = false;
        rigibody.AddRelativeForce(Vector3.back * 500 + Vector3.down * 250);

    }

    private void Update()
    {
        if (hasBeenThrown) {
            print("bottle is flying");
        }
    }
}
