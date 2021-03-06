using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * This component allows the player to move to a point in the screen by clicking it. 
 * Uses BFS to find the shortest path from the current location to the new location.
 */
public class ClickMoverAstar : TargetMoverAstar
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetTarget(newTarget);
        }
    }
}