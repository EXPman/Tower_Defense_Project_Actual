using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<GameTileScript> path;
    int destinationIndex;

    // Update is called once per frame
    void Update()
    {
        if (destinationIndex < path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[destinationIndex].transform.position,2 * Time.deltaTime);

            if(Vector3.Distance(transform.position, path[destinationIndex].transform.position) < 0.01f)
            {

            }
        }

         
    }

    internal void SetPath(List<GameTileScript> pathToGoal)
    {
        path = pathToGoal;
    }
}
