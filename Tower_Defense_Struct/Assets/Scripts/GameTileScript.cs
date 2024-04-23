using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GameTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        TowerManager towerManager = new TowerManager();

        if (towerManager.CanPlaceTowerHere() && !IsBlocked)
        {
            PlaceTower(towerManager.GetSelectedTower());
        }
    }

    private void PlaceTower(GameObject towerPrefab)
    {
        if (towerPrefab != null)
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            IsBlocked = true;
        }
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
        if (!IsBlocked)
        {
            if (GameManagerScript.gold >= TurretACost)
            {
                GameManagerScript.gold -= TurretACost; // Déduire le coût de la tour A
                TurretARenderer.enabled = true; // Activer le rendu pour la tour A
                IsBlocked = true; // Bloquer la tuile pour empêcher d'autres tours d'être placées ici
                GM.CalculateNewPath(); // Recalculer le chemin
            }
            else if (GameManagerScript.gold >= TurretBCost)
            {
                GameManagerScript.gold -= TurretBCost; // Déduire le coût de la tour B
                TurretBRenderer.enabled = true; // Activer le rendu pour la tour B
                IsBlocked = true; // Bloquer la tuile
                GM.CalculateNewPath(); // Recalculer le chemin
            }
            else
            {
                Debug.Log("Pas assez d'or pour placer une tour.");
            }
        }
        else
        {
            Debug.Log("Une tourelle est déjà présente sur cette tuile.");
        }

    }

    internal void SetPath(bool isPath)
    {
        spriteRenderer.color = isPath ? Color.green : originalColor;
    }
}
