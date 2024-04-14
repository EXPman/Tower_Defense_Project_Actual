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
            transform.position = Vector3.Lerp(transform.position
                , destination[destinationIndex].position, 2 * Time.deltaTime);

            if(Vector3.Distance(transform.position, destination[destinationIndex].position) < 0.01f)
            {
                destinationIndex++;
            }
        }
    }
}
