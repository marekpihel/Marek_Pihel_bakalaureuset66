using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneSuspicionManager : MonoBehaviour
{
    public static DroneSuspicionManager suspicionManager;
    List<DroneBehaviourController> drones;
    Vector3 lastPosition;
    int changeStateSoundAmount = 5;
    int timesSoundHeard = 0;
    float soundReactionDistance = 10f;
    float soundHeardReductionCooldown = 15f;
    float timeUntilSoundHeardReduction = 0f;

    void OnEnable()
    {
        drones = new List<DroneBehaviourController>();
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
        foreach (DroneBehaviourController droneController in FindObjectsOfType<DroneBehaviourController>())
        {
            droneController.ResetState();
            drones.Add(droneController);
        }
    }

    public void ResetTimesSoundHeard() {
        timesSoundHeard = 0;
    }

    public void heardSound(Vector3 soundHeardPosition) {
        ReloadDrones();
        lastPosition = soundHeardPosition;
        AlertCloseDrones();
        timesSoundHeard += 1;
        checkForAlterState();
    }

    private void AlertCloseDrones()
    {
        foreach (DroneBehaviourController droneController in drones)
        {
            if (droneController.InAlertRange(lastPosition, soundReactionDistance)) {
                droneController.ReactToSound(lastPosition);
            }
        }
    }

    private void checkForAlterState()
    {
        if (timesSoundHeard >= changeStateSoundAmount) {
            foreach (DroneBehaviourController droneController in drones) {
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

    public float GetRangeFromSoundToReact() { return soundReactionDistance; }


}
