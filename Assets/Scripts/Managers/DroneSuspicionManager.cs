using System.Collections.Generic;
using UnityEngine;

public class DroneSuspicionManager : MonoBehaviour
{
    public static DroneSuspicionManager suspicionManager;
    List<DroneMovementController> drones;
    Vector3 lastPosition;
    int changeStateSoundAmount = 2;
    int timesSoundHeard = 0;
    float soundHeardReductionCooldown = 5f;
    float timeUntilSoundHeardReduction = 0f;

    void OnEnable()
    {
        drones = new List<DroneMovementController>();
        if (suspicionManager == null)
        {
            suspicionManager = this;
            DontDestroyOnLoad(base.gameObject);
        }
        else
        {
            Destroy(base.gameObject);
        }
    }

    public void ReloadDrones() {
        drones.Clear();
        foreach (DroneMovementController droneController in FindObjectsOfType<DroneMovementController>())
        {
            drones.Add(droneController);
        }
    }

    public void ResetTimesSoundHeard() {
        timesSoundHeard = 0;
    }

    public void heardSound(Vector3 soundHeardPosition) {
        ReloadDrones();
        lastPosition = soundHeardPosition;
        timesSoundHeard += 1;
        checkForAlterState();
    }

    private void checkForAlterState()
    {
        if (timesSoundHeard >= changeStateSoundAmount) {
            foreach (DroneMovementController droneController in drones) {
                droneController.ChangeToSearchingState(lastPosition);
            }
        }
    }

    private void Update()
    {
        if (timeUntilSoundHeardReduction >= 0) {
            timeUntilSoundHeardReduction -= Time.deltaTime;
        } else {
            timeUntilSoundHeardReduction = soundHeardReductionCooldown;
            if (timesSoundHeard > 0) {
                timesSoundHeard -= 1;
            }
        }
    }

}
