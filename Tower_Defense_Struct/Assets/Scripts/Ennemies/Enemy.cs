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

    int hp = 3;

    private void Awake()
    {
        allEnnemies.Add(this);
    }

    //private void OnEnable()
    //{
    //    HP_Script.OnGameOver += DestroySelf;
    //}

    //private void OnDisable()
    //{
    //    HP_Script.OnGameOver -= DestroySelf;
    //}

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
        else if (!reachedEnd)
        {
            reachedEnd = true;
            OnEnemyReachedEnd?.Invoke();
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        if (allEnnemies.Contains(this))
        {
            Debug.Log("Destruction de l'ennemi.");
            allEnnemies.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("L'ennemi a déjà été retiré de la liste.");
        }
    }

    internal void Attack()
    {
        Debug.Log("Attaque en cours, HP avant attaque: " + hp);
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
