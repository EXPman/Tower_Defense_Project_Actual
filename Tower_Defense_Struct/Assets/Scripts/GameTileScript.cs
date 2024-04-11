using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;


    [SerializeField] SpriteRenderer HoverRenderer;
    [SerializeField] SpriteRenderer SpawnerRenderer;

    public GameManagerScript GM { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBlocked { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    internal void TurnGrey()
    {
        spriteRenderer.color = Color.gray;
        originalColor = spriteRenderer.color; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverRenderer.enabled = true;
        GM.TargetTile = this;
        Debug.Log("enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverRenderer.enabled = false;
        Debug.Log("exit");
    }

    public void onPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");

    }

    internal void SetEnemySpawn()
    {
        SpawnerRenderer.enabled = true;

    }
    internal void SetPath(bool isPath)
    {
      spriteRenderer.color = isPath ? Color.green : originalColor;
    }
   
}
