using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerScript : MonoBehaviour
{
    List<Enemy> AlliesInRange = new List<Enemy>();
    public static float HealWaitTime = 1;
    public static int HPHealed = 3;

    [SerializeField] CircleCollider2D Range;

    private void Awake()
    {
        Range.enabled = true;
        StartCoroutine(HealCoroutine());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy friend = collision.GetComponent<Enemy>();
        Debug.Log("in");
        if (!AlliesInRange.Contains(friend))
        {
            AlliesInRange.Add(friend);
            Debug.Log("in");
        }
    }

    IEnumerator HealCoroutine()
    {
        for (int i = 0; i < AlliesInRange.Count; i++)
        {
            AlliesInRange[i].hp += HPHealed;
        }

        yield return new WaitForSeconds(HealWaitTime);
    }
}
