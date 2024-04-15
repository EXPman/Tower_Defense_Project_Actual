using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] SpriteRenderer HoverRenderer;
    [SerializeField] SpriteRenderer TurretRenderer;
    [SerializeField] SpriteRenderer SpawnerRenderer;

    public GameManagerScript GM { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBlocked { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TurretRenderer.enabled = false; 
    }

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    internal void TurnGrey()
    {
        spriteRenderer.color = Color.gray;
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


    internal void SetEnemySpawn()
    {
        SpawnerRenderer.enabled = true;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        TurretRenderer.enabled = !TurretRenderer.enabled;
        IsBlocked = TurretRenderer.enabled;
    }
    
    internal void SetPath(bool isPath)
    {
        spriteRenderer.color = isPath ? Color.green : originalColor;
    }
}
