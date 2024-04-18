using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private const int TurretCost = 25;

    [SerializeField] SpriteRenderer HoverRenderer;
    [SerializeField] SpriteRenderer TurretRenderer;
    [SerializeField] SpriteRenderer SpawnerRenderer;
    private LineRenderer lineRenderer;
    private bool canAttack = true;

    //Enemy target = null;

    public GameManagerScript GM { get; set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBlocked { get; private set; }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0,transform.position);
        spriteRenderer = GetComponent<SpriteRenderer>();
        TurretRenderer.enabled = false; 
    }

    private void Update()
    {
        if(TurretRenderer.enabled && canAttack)
        {
            Enemy target = null;
            foreach (var ennemy in Enemy.allEnnemies)
            {
                if(Vector3.Distance(transform.position,ennemy.transform.position)<2)
                {
                    target = ennemy;
                    break;
                }
            }

            if (target != null)
            {
                StartCoroutine(AttackCoroutine(target));
            }
        }
    }


    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    IEnumerator AttackCoroutine(Enemy target)
    {
        target.GetComponent<Enemy>().Attack();
        canAttack = false;
        lineRenderer.SetPosition(1, target.transform.position); 
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.3f); 
        canAttack = true;
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
        IsBlocked = TurretRenderer.enabled;

        if(GM.gold >= 25)
        {
            GM.gold -= TurretCost;
            TurretRenderer.enabled = true;
        }
           

    }

    internal void SetPath(bool isPath)
    {
        spriteRenderer.color = isPath ? Color.green : originalColor;
    }
}
