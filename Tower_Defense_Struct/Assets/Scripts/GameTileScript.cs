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

    [SerializeField] SpriteRenderer HoverRenderer;
    [SerializeField] SpriteRenderer TurretARenderer;
    [SerializeField] SpriteRenderer TurretBRenderer;
    [SerializeField] SpriteRenderer SpawnerRenderer;
    private LineRenderer lineRenderer;
    private bool canAttack = true;

    public static float TurretRange = 2;
    public static float TurretAttackSpeed = 0.2f;

    //Enemy target = null;

    public GameManagerScript GM { get; set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBlocked { get; private set; }

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
        if (TurretARenderer.enabled && canAttack)
        {
            Enemy target = null;
            foreach (var ennemy in Enemy.allEnnemies)
            {
                if (Vector3.Distance(transform.position, ennemy.transform.position) < TurretRange)
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
        target.Attack();
        //target.GetComponent<Enemy>().Attack();
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

        if (IsBlocked)
        {
            Debug.Log("Une tourelle est déjà présente sur cette tuile.");
            return;
        }

        GameObject selectedTower = towerManager.GetSelectedTower();

        if (selectedTower != null)
        {
            int cost = selectedTower == towerManager.towerPrefebs[0] ? TurretACost : TurretBCost;
            if (GameManagerScript.gold >= cost)
            {
                GameManagerScript.gold -= cost;
                Instantiate(selectedTower, transform.position, Quaternion.identity);
                IsBlocked = true;
                GM.CalculateNewPath();
                towerManager.SelectTower(-1); // Désélectionner la tour

                // Ajout pour gérer le retour à l'état de hover par défaut
                HoverRenderer.enabled = false; // Désactiver le rendu de hover
                if (selectedTower == towerManager.towerPrefebs[0])
                {
                    TurretARenderer.enabled = true;
                }
                else
                {
                    TurretBRenderer.enabled = true;
                }
            }
            else
            {
                Debug.Log("Pas assez d'or pour placer une tour.");
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
}
