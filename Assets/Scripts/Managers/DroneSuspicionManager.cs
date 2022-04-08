using System.Collections.Generic;
using UnityEngine;

public class DroneSuspicionManager : MonoBehaviour
{
    public static DroneSuspicionManager suspicionManager;
    List<DroneBehaviourController> allDrones;
    List<DroneBehaviourController> dronesReactingToSound;
    Vector3 lastPosition;
    int changeStateSoundAmount = 5;
    int timesSoundHeard = 0;
    float soundReactionDistance = 10f;
    float soundHeardReductionCooldown = 15f;
    float timeUntilSoundHeardReduction = 0f;

    void OnEnable()
    {
        allDrones = new List<DroneBehaviourController>();
        dronesReactingToSound = new List<DroneBehaviourController>();
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

    public void ReloadDrones()
    {
        allDrones.Clear();
        foreach (DroneBehaviourController droneController in FindObjectsOfType<DroneBehaviourController>())
        {
            droneController.ResetState();
            allDrones.Add(droneController);
        }
    }

    public void ResetTimesSoundHeard()
    {
        timesSoundHeard = 0;
    }

    public void heardSound(Vector3 soundHeardPosition, DroneBehaviourController droneController)
    {
        ReloadDrones();
        lastPosition = soundHeardPosition;
        AddDroneReactingToSound(droneController);
        AlertCloseDrones();
        timesSoundHeard += 1;
        checkForAlterState();
    }

    private void AlertCloseDrones()
    {
        foreach (DroneBehaviourController droneController in allDrones)
        {
            if (droneController.InAlertRange(lastPosition, soundReactionDistance) && ShouldReactToSound())
            {
                droneController.ReactToSound(lastPosition);
                AddDroneReactingToSound(droneController);
            }
        }
    }

    private void checkForAlterState()
    {
        if (timesSoundHeard >= changeStateSoundAmount)
        {
            foreach (DroneBehaviourController droneController in allDrones)
            {
                droneController.ChangeToSearchingState(lastPosition);
            }
        }
    }

    private void Update()
    {
        if (timeUntilSoundHeardReduction >= 0)
        {
            timeUntilSoundHeardReduction -= Time.deltaTime;
        }
        else
        {
            timeUntilSoundHeardReduction = soundHeardReductionCooldown;
            if (timesSoundHeard > 0)
            {
                timesSoundHeard -= 1;
            }
        }
    }

    public float GetRangeFromSoundToReact() { return soundReactionDistance; }

    public void AddDroneReactingToSound(DroneBehaviourController droneController) {
        dronesReactingToSound.Add(droneController);
    }

    public bool ShouldReactToSound() {
        return dronesReactingToSound.Count < timesSoundHeard;
    }

    public void RemoveDroneFromReacting(DroneBehaviourController droneController) {
        dronesReactingToSound.Remove(droneController);
    }


}
