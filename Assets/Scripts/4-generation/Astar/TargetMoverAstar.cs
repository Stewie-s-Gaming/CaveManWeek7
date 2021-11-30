using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetMoverAstar : MonoBehaviour
{
    [SerializeField] Tilemap tilemap = null;
    [Tooltip("The speed by which the object moves towards the target, in meters (=grid units) per second")]
    [SerializeField] float speed = 2f;
    [Tooltip("Maximum number of iterations before BFS algorithm gives up on finding a path")]
    [SerializeField] int maxIterations = 1000;
    [Tooltip("The target position in world coordinates")]
    [SerializeField] Vector3 targetInWorld;
    [Tooltip("The target position in grid coordinates")]
    [SerializeField] Vector3Int targetInGrid;
    [SerializeField] TileBase tilebase;
    [SerializeField] AllowedTiles allowedTiles;
    protected bool atTarget;  // This property is set to "true" whenever the object has already found the target.
    private float timeBetweenSteps=0.1f;
    public void SetTarget(Vector3 newTarget)
    {
        if (targetInWorld != newTarget && allowedTarget(newTarget))
        {
            targetInWorld = newTarget;
            targetInGrid = tilemap.WorldToCell(targetInWorld);
            atTarget = false;
        }
    }
    public Vector3 GetTarget()
    {
        return targetInWorld;
    }

    public bool allowedTarget(Vector3 pos)
    {
        Vector3Int tilePos= tilemap.WorldToCell(pos);
        if (allowedTiles.Contain(tilemap.GetTile(tilePos)))
        {
            return true; 
        }
        Debug.Log("You can't reach unallowed tile!");
        return false;
    }
    protected virtual void Start()
    {
        atTarget = true;
        StartCoroutine(MoveTowardsTheTarget());
    }

    IEnumerator MoveTowardsTheTarget()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(timeBetweenSteps);
            if (enabled && !atTarget)
                MakeOneStepTowardsTheTarget();
        }
    }

    private void MakeOneStepTowardsTheTarget()
    {
        Vector3Int startNode = tilemap.WorldToCell(transform.position);
        Vector3Int endNode = targetInGrid;
        Astar astarObject = new Astar(startNode, endNode, tilemap, maxIterations, tilebase,allowedTiles);
        List<Vector3Int> shortestPath = astarObject.GetPath();
        Debug.Log("shortestPath = " + string.Join(" , ", shortestPath));
        if (shortestPath.Count >= 2)
        {
            Vector3Int nextNode = shortestPath[1];
            transform.position = tilemap.GetCellCenterWorld(nextNode);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else
        {
            atTarget = true;
        }
    }
}
