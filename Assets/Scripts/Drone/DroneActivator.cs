using System.Collections;
using UnityEngine;

public class DroneActivator : MonoBehaviour
{
    [SerializeField]
    GameObject drone;
    [SerializeField]
    float droneSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        drone.SetActive(false);
        StartCoroutine(SpawnDrone());   
    }

    IEnumerator SpawnDrone()
    {
        yield return new WaitForSeconds(droneSpawnTime);
        drone.SetActive(true);
    }
}
