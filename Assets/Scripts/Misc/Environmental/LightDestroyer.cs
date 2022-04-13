using UnityEngine;

public class LightDestroyer : MonoBehaviour
{
    [SerializeField]
    float lightDestroyTime;

    // Update is called once per frame
    void Update()
    {
        if (lightDestroyTime <= 0)
        {
            Destroy(this.gameObject);
        }
        else {
            lightDestroyTime -= Time.deltaTime;
        }
    }
}
