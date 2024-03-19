using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Skill_Tree : MonoBehaviour
{
    [SerializeField] Node_Skill_Tree ParentNode;
    LineRenderer lineRenderer;

    private void Awake()
    {
        if (ParentNode != null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0,this.transform.position);
            lineRenderer.SetPosition(1,ParentNode.transform.position);

        }


    }


}
