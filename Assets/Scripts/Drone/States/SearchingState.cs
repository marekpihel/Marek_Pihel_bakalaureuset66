using UnityEngine;

public class SearchingState : State
{
    Vector3 pointOfIntetest;
    int searchRadius;
    float investigationCooldown = 3, investigatedTime = 0;


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
            investigatedTime -= Time.deltaTime;
        }
    }

    #region Expand search area
    private void ExpandSearchArea()
    {
        searchRadius += 1;
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
        else {
            return false;
        }
    }
    #endregion

    #region Setters
    public void SetPointOfInterest(Vector3 newPointOfInterest) {
        this.pointOfIntetest = newPointOfInterest;
    }

    public void SetSearchRadius(int searchRadius) {
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
        return UnityEngine.Random.Range(-1*searchRadius, searchRadius);
    }
    #endregion
}
