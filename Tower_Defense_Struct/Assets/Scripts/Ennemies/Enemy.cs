using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<GameTileScript> path;
    int destinationIndex;

    internal void SetPath(List<GameTileScript> pathToGoal)
    {
        path = pathToGoal;
    }

    // Update is called once per frame
    void Update()
    {
        if(destinationIndex < path.Count)
        {
            Vector3 destPos = path[destinationIndex].transform.position;
            transform.position += Vector3.MoveTowards(transform.position, destPos, 2 * Time.deltaTime);

            if (Vector3.Distance(transform.position, destPos) < 0.01)
            {
                destinationIndex++;
            }
        }
    }
}
