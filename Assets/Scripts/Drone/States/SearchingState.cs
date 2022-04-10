using UnityEngine;

public class SearchingState : State
{
    Vector3 pointOfIntetest;
    int searchRadius = 1;
    float investigationCooldown = 3, investigatedTime = 0, turningSpeed = 50;

    public SearchingState(string stateName)
    {
        base.SetName(stateName);
    }

    public override void PerformAction()
    {
        if (!GetNavMeshAgent().hasPath)
        {
            if (ShouldGetNewPoint())
            {
                ExpandSearchArea();
                Vector3 nextDest = GetRandomPointInsideSearchArea();
                GoToLocation(nextDest);
            }
            if (investigatedTime >= 0) { 
                LookAround();
            }
            investigatedTime -= Time.deltaTime;
        }
    }

    #region Initialize search parameters
    internal void InitializeSearchParameters(Vector3 pointOfInterest, int searchRadius)
    {
        GetNavMeshAgent().ResetPath();
        pointOfIntetest = pointOfInterest;
        SetSearchRadius(searchRadius);
    }
    #endregion

    #region Expand search area
    private void ExpandSearchArea()
    {
        searchRadius += 2;
    }
    #endregion

    #region Look around
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

    #region Should get new point
    private bool ShouldGetNewPoint()
    {
        if (investigatedTime <= 0)
        {
            investigatedTime = investigationCooldown;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Setters
    public void SetPointOfInterest(Vector3 newPointOfInterest)
    {
        this.pointOfIntetest = newPointOfInterest;
    }

    public void SetSearchRadius(int searchRadius)
    {
        this.searchRadius = searchRadius;
    }
    #endregion

    #region Get random point for searching
    private Vector3 GetRandomPointInsideSearchArea()
    {
        return pointOfIntetest + new Vector3(GetRandomCoordInsideSearchRadius(searchRadius), 0, GetRandomCoordInsideSearchRadius(searchRadius));
    }

    private float GetRandomCoordInsideSearchRadius(int searchRadius)
    {
        return UnityEngine.Random.Range(-1 * searchRadius, searchRadius);
    }
    #endregion
}
