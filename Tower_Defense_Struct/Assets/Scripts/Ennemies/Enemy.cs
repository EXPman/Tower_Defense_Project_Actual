using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject visual;
    public GameManagerScript GM;
    private Stack<GameTileScript> path = new Stack<GameTileScript>();
    public static event Action OnEnemyReachedEnd;
    public static HashSet<Enemy> allEnnemies = new HashSet<Enemy>();

    int hp = 10; 

    private void Awake()
    {
        allEnnemies.Add(this);
    }

    private void OnEnable()
    {
        HP_Script.OnGameOver += DestroySelf;
    }

    private void OnDisable()
    {
        HP_Script.OnGameOver -= DestroySelf;
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
            transform.position = Vector3.MoveTowards(transform.position, destPos, 2 * Time.deltaTime);

            if (Vector3.Distance(transform.position, destPos) < 0.01f)
            {
                path.Pop();

            }

        }
        else
        {
            OnEnemyReachedEnd?.Invoke();
            DestroySelf();
            //allEnnemies.Remove(this);
            //Destroy(gameObject);
        }
    }

    private void DestroySelf()
    {
        allEnnemies.Remove(this);
        Destroy(gameObject);
    }

    internal void Attack()
    {
        if(--hp <= 0)
        {
            GM.gold++;
            DestroySelf(); 
        }
        else
        {
            visual.transform.localScale *= 0.9f; 
        }
    }
}
