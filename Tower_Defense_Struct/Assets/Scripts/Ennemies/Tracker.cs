using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] List<Transform> destination = new List<Transform>();
    int destinationIndex; 

    // Update is called once per frame
    void Update()
    {
        if(destinationIndex < destination.Count)
        {
            Vector3 destPos = destination[destinationIndex].position;
             transform.position= Vector3.MoveTowards(transform.position
                , destPos, 2 * Time.deltaTime);

            if(Vector3.Distance(transform.position, destPos) < 0.01f)
            {
                destinationIndex++;
            }
        }
    }
}
