using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Skill_Tree : MonoBehaviour
{
    [SerializeField] Node_Skill_Tree ParentNode;
    LineRenderer lineRenderer;
    [SerializeField] public List<Node_Skill_Tree> ChildNodes = new List<Node_Skill_Tree>();
    [SerializeField] SpriteRenderer Sprite;

    public enum NodeState
    {
        Claimed,
        available,
        Locked
    }

    NodeState CurrentStatus = NodeState.Locked;

    private void Awake()
    {
        if (ParentNode != null)
        {
            ParentNode.ChildNodes.Add(this);
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0,this.transform.position);
            lineRenderer.SetPosition(1,ParentNode.transform.position);
            SetState(NodeState.Locked);
        }
        else
        {
            SetState(NodeState.available);
        }

    }

    private void OnMouseDown()
    {
        if(CurrentStatus == NodeState.available)
        {
            SetState(NodeState.Claimed);
            if(ChildNodes != null)
            {
                foreach(Node_Skill_Tree STNode in ChildNodes)
                {
                    STNode.SetState(NodeState.available);
                }
            }
        }
    }

    private void SetState(NodeState State)
    {
        CurrentStatus = State;
        switch(CurrentStatus)
        {
            case NodeState.available:
                Sprite.color = Color.yellow;
                break;
            case NodeState.Claimed:
                Sprite.color = Color.green;
                break;
            case NodeState.Locked:
                Sprite.color = Color.red;
                break;
        }
    }
}
