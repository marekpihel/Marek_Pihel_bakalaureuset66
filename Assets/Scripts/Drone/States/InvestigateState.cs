using UnityEngine;

public class InvestigateState : State
{
    Vector3 pointOfIntetest;
    int searchRadius = 1, maxSearchAmount = 3, searchedTimes = 3;
    float investigationCooldown = 3, investigatedTime = 0, turningSpeed = 50;

    public InvestigateState(string stateName)
    {
        base.SetName(stateName);
    }

    public override void PerformAction()
    {
        if (!GetNavMeshAgent().hasPath)
        {
            if (ShouldGetNewPoint())
            {
                Vector3 nextDest = GetRandomPointInsideSearchArea();
                GoToLocation(nextDest);
            }
            if (investigatedTime >= 0)
            {
                LookAround();
            }
            investigatedTime -= Time.deltaTime;
        }
    }

    #region Looking around
    private void LookAround()
    {
        Transform droneTransform = GetNavMeshAgent().gameObject.transform;
        if (investigatedTime > investigationCooldown * 2 / 3)
        {
            droneTransform.Rotate(Vector3.up, turningSpeed * Time.deltaTime);
        }
        else
        {
            droneTransform.Rotate(Vector3.down, turningSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region Should we get a new point
    private bool ShouldGetNewPoint()
    {
        if (investigatedTime <= 0 && searchedTimes > 0)
        {
            investigatedTime = investigationCooldown;
            searchedTimes -= 1;
            return true;
        }
        else if (searchedTimes == 0)
        {
            investigatedTime = 0;
            SetIsFinished(true);
            return false;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region  Setters
    public void SetPointOfInterest(Vector3 newPointOfInterest)
    {
        pointOfIntetest = newPointOfInterest;
    }

    public void SetSearchRadius(int searchRadius)
    {
        this.searchRadius = searchRadius;
    }
    #endregion

    #region Initialize search parameters
    public void InitializeSearchParameters(Vector3 pointOfInterest, int searchRadius)
    {
        GetNavMeshAgent().ResetPath();
        searchedTimes = maxSearchAmount;
        investigatedTime = 0;
        SetIsFinished(false);
        pointOfIntetest = pointOfInterest;
        SetSearchRadius(searchRadius);
    }
    #endregion

    #region Get random point for search
    private Vector3 GetRandomPointInsideSearchArea()
    {
        return pointOfIntetest + new Vector3(GetRandomCoordInsideSearchRadius(searchRadius), 0, GetRandomCoordInsideSearchRadius(searchRadius));
    }

    private float GetRandomCoordInsideSearchRadius(int searchRadius)
    {
        return Random.Range(-1 * searchRadius, searchRadius);
    }
    #endregion
}
