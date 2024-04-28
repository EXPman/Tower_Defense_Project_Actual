using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GameTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public static int TurretACost = 25;
    public static int TurretBCost = 40;
    public static int TurretCCost = 55;
    public static int TurretDCost = 35;
    public static int TurretECost = 70;

    public int CurrentCost = 25;

    [SerializeField] SpriteRenderer HoverRenderer;
    [SerializeField] SpriteRenderer TurretARenderer;
    [SerializeField] SpriteRenderer TurretBRenderer;
    [SerializeField] SpriteRenderer TurretCRenderer;
    [SerializeField] SpriteRenderer TurretDRenderer;
    [SerializeField] SpriteRenderer TurretERenderer;
    [SerializeField] public SpriteRenderer SpawnerRenderer;
    [SerializeField] SpriteRenderer WallRenderer;
    private LineRenderer lineRenderer;
    private bool canAttack = true;

    public static float RangeBuff = 0;
    public static float AttackSpeedBuff = 1;

    public int TurretDamage = 3;
    public float AttackSpeed = 1.5f;
    public float Range = 2;

    //Enemy target = null;

    public GameManagerScript GM { get; set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBlocked { get; private set; }
    public bool IsWall { get; private set; }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, transform.position);
        spriteRenderer = GetComponent<SpriteRenderer>();
        TurretARenderer.enabled = false;
        TurretBRenderer.enabled = false;
    }

    private void Update()
    {
        if (this.tag!="Untagged" && canAttack)
        {
            Enemy target = null;
            foreach (var ennemy in Enemy.allEnnemies)
            {
                if (Vector3.Distance(transform.position, ennemy.transform.position) < (Range+RangeBuff))
                {
                    if (ennemy.tag == "Camo")
                    {
                        if (this.tag != "TurretA")
                        {
                            target = ennemy;
                            break;
                        }
                    }
                    else if(ennemy.tag == "Flying")
                    {
                        if(this.tag != "TurretA")
                        {
                            target = ennemy;
                            break;
                        }
                    }
                    else
                    {
                        target = ennemy;
                        break;
                    }
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
        target.Attack(TurretDamage,this.tag);
        //target.GetComponent<Enemy>().Attack();
        canAttack = false;
        lineRenderer.SetPosition(1, target.transform.position);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(AttackSpeed*AttackSpeedBuff);
        canAttack = true;
    }

    internal void TurnGrey()
    {
        spriteRenderer.color = Color.gray;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverRenderer.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverRenderer.enabled = false;
    }

    internal void SetEnemySpawn()
    {
        SpawnerRenderer.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TowerManager towerManager = FindAnyObjectByType<TowerManager>();
        int selectedTower = towerManager.GetSelectedTower();

        if (selectedTower != -1)
        {
            if (WallRenderer.enabled)
            {
                switch(TowerManager.Singleton.CurrentTowerIndex)
                {
                    case 0:
                        break;
                    case 1:
                        CurrentCost = 40;
                        break;
                    case 2:
                        CurrentCost = 55;
                        break;
                    case 3:
                        CurrentCost = 35;
                        break;
                    case 4:
                        CurrentCost = 70;
                        break;
                }


                if (GameManagerScript.gold >= CurrentCost)
                {
                    GameManagerScript.gold -= CurrentCost;
                    IsBlocked = true;
                    GM.CalculateNewPath();
                    towerManager.SelectTower(-1); // Désélectionner la tour

                    // Ajout pour gérer le retour à l'état de hover par défaut
                    HoverRenderer.enabled = false; // Désactiver le rendu de hover
                    switch(selectedTower)
                    {
                        case 0:
                            TurretARenderer.enabled = true;
                            break;
                        case 1:
                            TurretBRenderer.enabled = true;
                            break;
                        case 2:
                            TurretCRenderer.enabled = true;
                            break;
                        case 3:
                            TurretDRenderer.enabled = true;
                            break;
                        case 4:
                            TurretERenderer.enabled = true;
                            break;
                    }
                    TowerType.Instance.SetType(this);
                }
                else
                {
                    Debug.Log("Pas assez d'or pour placer une tour.");
                }
            }
            else
            {
                Debug.Log("Les ne sont posable que sur les murs");
            }
        }
        else
        {
            Debug.Log("Aucune tour sélectionnée. Sélectionnez une tour avant de placer.");
        }
    }

    internal void SetPath(bool isPath)
    {
        spriteRenderer.color = isPath ? Color.green : originalColor;
    }

    internal void SetWall()
    {
        WallRenderer.enabled = true;
        IsWall = true;
        IsBlocked = true; 
    }
}
