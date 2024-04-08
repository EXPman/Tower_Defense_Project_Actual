using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer HoverRenderer;
    [SerializeField] SpriteRenderer SpawnerRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    internal void TurnGrey()
    {
        spriteRenderer.color = Color.gray;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverRenderer.enabled = true;
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
}
