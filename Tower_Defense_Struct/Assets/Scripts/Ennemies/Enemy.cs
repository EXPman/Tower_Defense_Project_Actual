using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }
}
