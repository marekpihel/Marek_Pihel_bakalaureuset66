using System.Collections.Generic;
using UnityEngine;

public class DroneSuspicionManager : MonoBehaviour
{
    public static DroneSuspicionManager suspicionManager;
    List<DroneBehaviourController> allDrones;
    Vector3 lastPosition;
    int changeStateSoundAmount = 5, shortTimeSoundHeard = 0, searchRadius = 5, totalTimesSoundHeard = 0;
    float soundReactionDistance = 10f, soundHeardReductionCooldown = 60f, timeUntilSoundHeardReduction = 0f;

    void OnEnable()
    {
        allDrones = new List<DroneBehaviourController>();
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

    public void ResetDroneSuspicionParams()
    {
        shortTimeSoundHeard = 0;
        totalTimesSoundHeard = 0;
        searchRadius = 5;
    }

    public void HeardSound(Vector3 soundHeardPosition, DroneBehaviourController droneController)
    {
        ReloadDrones();
        lastPosition = soundHeardPosition;
        shortTimeSoundHeard += 1;
        totalTimesSoundHeard += 1;
        ExpandSearchRadius();
        AlertCloseDrones();
        checkForAlterState();
    }

    public void ExpandSearchRadius()
    {
        if (ShouldExpandSearchRadius())
        {
            searchRadius += 1;
            print("Increasing range");
            print("New range: " + searchRadius);
        }
    }

    private bool ShouldExpandSearchRadius()
    {
        if (totalTimesSoundHeard % 3 == 0)
        {
            return true;
        }
        return false;
    }

    private void AlertCloseDrones()
    {
        int count = 0;
        foreach (DroneBehaviourController droneController in allDrones)
        {
            if (droneController.InAlertRange(lastPosition, soundReactionDistance))
            {
                droneController.ReactToSound(lastPosition);
                count += 1;
            }
        }
        print("Drones reacting: " + count);
    }

    private void checkForAlterState()
    {
        if (shortTimeSoundHeard >= changeStateSoundAmount)
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
            if (shortTimeSoundHeard > 0)
            {
                shortTimeSoundHeard -= 1;
            }
        }
    }

    public float GetRangeFromSoundToReact()
    {
        return soundReactionDistance;
    }

    public int GetSearchReadius()
    {
        return searchRadius;
    }

}
