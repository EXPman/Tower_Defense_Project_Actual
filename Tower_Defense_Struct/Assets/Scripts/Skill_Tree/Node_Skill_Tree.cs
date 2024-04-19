using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node_Skill_Tree : MonoBehaviour
{
    [SerializeField] Node_Skill_Tree ParentNode;
    LineRenderer lineRenderer;
    [SerializeField] public List<Node_Skill_Tree> ChildNodes = new List<Node_Skill_Tree>();
    [SerializeField] SpriteRenderer Sprite;
    [SerializeField] TMP_Text Bufftext;
    [SerializeField] int BuffValue = 1;

    public enum NodeState
    {
        Claimed,
        available,
        Locked
    }

    NodeState CurrentStatus = NodeState.Locked;

    private void Awake()
    {
        switch(this.tag)
        {
            case "HPNode":
                Bufftext.text = $"+{BuffValue} HP";
                break;
            case "RangeNode":
                Bufftext.text = $"+{BuffValue} Range";
                break;
        }
        
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
                switch (this.tag)
                {
                    case "HPNode":
                        HP_Script.BonusHP += BuffValue;
                        break;
                    case "RangeNode":
                        GameTileScript.TurretRange += BuffValue;
                        break;
                }
                Sprite.color = Color.green;
                break;
            case NodeState.Locked:
                Sprite.color = Color.red;
                break;
        }
    }
}
