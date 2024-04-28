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

    public int hp = 3;
    public float speed = 2;
    public int GoldDrop = 1;

    [SerializeField] SpriteRenderer ClassicSprite;
    [SerializeField] SpriteRenderer ResistantSprite;
    [SerializeField] SpriteRenderer CamoSprite;
    [SerializeField] SpriteRenderer HealerSprite;
    [SerializeField] SpriteRenderer SprinterSprite;
    [SerializeField] SpriteRenderer FlyingSpriteSprite;

    private void Awake()
    {
        EnnemyTypes.Singleton.SetType(this);
        switch(this.tag)
        {
            case "Ennemyhealer":
                this.GetComponent<HealerScript>().enabled = true;
                break;
            case "flying":
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
    }

    internal void SetPath(List<GameTileScript> pathToGoal)
    {
        path.Clear();
        foreach (GameTileScript tile in pathToGoal)
        {
            path.Push(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count > 0)
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

    internal void Attack()
    {
        if (--hp <= 0)
        {
            GameManagerScript.gold++;
            DestroySelf(); 
        }
        else
        {
            visual.transform.localScale = new Vector3((float)(visual.transform.localScale.x * 0.9), (float)(visual.transform.localScale.x * 0.9), (float)(visual.transform.localScale.x * 0.9));
            Debug.Log("ennemy hit");
        }
    }
}
