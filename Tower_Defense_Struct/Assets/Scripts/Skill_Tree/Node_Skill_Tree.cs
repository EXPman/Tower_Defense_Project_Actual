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
    [SerializeField] float BuffValue = 1f;
    private int Price;

    public enum NodeState
    {
        Claimed,
        available,
        Locked
    }

    NodeState CurrentStatus = NodeState.Locked;

    private void Awake()
    {
        switch (this.tag)
        {
            case "HPNode":
                BuffValue = 5;
                break;
            case "RangeNode":
                BuffValue = 0.5f;
                break;
            case "ASPNode":
                BuffValue = 0.9f;
                break;
        }

        switch (this.tag)
        {
            case "HPNode":
                Bufftext.text = $"+{BuffValue} HP";
                break;
            case "RangeNode":
                Bufftext.text = $"+{BuffValue} Range";
                break;
            case "ASPNode":
                Bufftext.text = $"*{BuffValue} ASP";
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
            if(EXPScript.EXP >= Price)
            {
                SetState(NodeState.available);
            }
        }

    }

    private void OnMouseDown()
    {
        if(CurrentStatus == NodeState.available)
        {
            SetState(NodeState.Claimed);
            if(ChildNodes != null && EXPScript.EXP >= Price)
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
                        GameTileScript.RangeBuff += BuffValue;
                        break;
                    case "ASPNode":
                        GameTileScript.AttackSpeedBuff *= BuffValue;
                        break;

                }
                Sprite.color = Color.green;
                EXPScript.EXP -= Price;
                break;
            case NodeState.Locked:
                Sprite.color = Color.red;
                break;
        }
    }
}
