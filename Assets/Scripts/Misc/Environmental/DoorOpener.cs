using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField]
    float openingTime;

    // Update is called once per frame
    void Update()
    {
        if (openingTime > 0)
        {
            openingTime -= Time.deltaTime;
        }
        else {
            if (gameObject.transform.position.y < 5.5) {
                gameObject.transform.Translate(Vector3.up * Time.deltaTime);
            }
        }
    }
}
