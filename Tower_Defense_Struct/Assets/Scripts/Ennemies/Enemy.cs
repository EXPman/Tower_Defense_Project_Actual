using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject visual;
    private Stack<GameTileScript> path = new Stack<GameTileScript>();
    public static event Action OnEnemyReachedEnd;
    public static HashSet<Enemy> allEnnemies = new HashSet<Enemy>();
    private bool reachedEnd = false;

    public float InitialSpeed;

    public int hp = 3;
    public float speed = 2;
    public int GoldDrop = 1;

    private int freezeCounter = 0;

    [SerializeField] SpriteRenderer ClassicSprite;
    [SerializeField] SpriteRenderer ResistantSprite;
    [SerializeField] SpriteRenderer CamoSprite;
    [SerializeField] SpriteRenderer HealerSprite;
    [SerializeField] SpriteRenderer SprinterSprite;
    [SerializeField] SpriteRenderer FlyingSpriteSprite;

    public static event Action<Enemy> OnEnemyDestroyed;

    public Transform TargetTile;  // Target tile for navigation

    private void Awake()
    {
        EnnemyTypes.Singleton.SetType(this);
        switch (this.tag)
        {
            case "Ennemyhealer":
                this.GetComponent<HealerScript>().enabled = true;
                break;
            case "Flying":
                this.GetComponent<FlyingScript>().enabled = true;
                break;
            default:
                break;
        }
        switch (this.tag)
        {
            case "Resitant": // resistant
                ClassicSprite.enabled = false;
                ResistantSprite.enabled = true;
                break;
            case "Camo": //camo
                ClassicSprite.enabled = false;
                CamoSprite.enabled = true;
                break;
            case "EnnemyHealer": //healer
                ClassicSprite.enabled = false;
                HealerSprite.enabled = true;
                break;
            case "Sprinter": //sprinter
                ClassicSprite.enabled = false;
                SprinterSprite.enabled = true;
                break;
            case "Flying": //flying
                ClassicSprite.enabled = false;
                FlyingSpriteSprite.enabled = true;
                break;
            default: //classic
                break;
        }
        allEnnemies.Add(this);
        InitialSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (freezeCounter > 0)
        {
            freezeCounter--;
        }
        else
        {
            speed = InitialSpeed;
        }
    }

    internal void SetPath(List<GameTileScript> pathToGoal)
    {
        path.Clear();
        foreach (GameTileScript tile in pathToGoal)
        {
            path.Push(tile);
        }
    }

    void Update()
    {
        if (tag == "Flying" && TargetTile != null)
        {
            // Move flying enemies directly towards the target tile
            transform.position = Vector3.MoveTowards(transform.position, TargetTile.position, speed * Time.deltaTime);
        }
        else if (path.Count > 0)
        {
            Vector3 destPos = path.Peek().transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destPos) < 0.01f)
            {
                path.Pop();
            }
        }
        else if (!reachedEnd)
        {
            reachedEnd = true;
            OnEnemyReachedEnd?.Invoke();
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        allEnnemies.Remove(this);
        Destroy(gameObject);
    }

    internal void Attack(int Damage, string TurretType)
    {
        hp -= Damage;
        if (hp <= 0)
        {
            GameManagerScript.gold += GoldDrop;
            DestroySelf();
        }
        else
        {
            visual.transform.localScale = new Vector3((float)(visual.transform.localScale.x * 0.9), (float)(visual.transform.localScale.x * 0.9), (float)(visual.transform.localScale.x * 0.9));
            if (TurretType == "TurretD")
            {
                speed /= 2;
                freezeCounter = 60;
            }
        }
    }

}



