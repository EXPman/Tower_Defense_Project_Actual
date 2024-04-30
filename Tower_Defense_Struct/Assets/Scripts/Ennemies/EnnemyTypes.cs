using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTypes : MonoBehaviour
{
    public static EnnemyTypes Singleton;
    



    void Awake()
    {
        //makes sure the script is singleton
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetType(Enemy E)
    {
        //switch(Random.Range(0,6))
        switch (4)
        {
            case 0: //classic
                break;
            case 1: // resistant
                E.hp = 20;
                E.speed = 0.8f;
                E.GoldDrop = 6;
                E.tag = "Resitant";
                break;
            case 2: //camo
                E.hp = 7;
                E.speed = 1.6f;
                E.GoldDrop = 10;
                E.tag = "Camo";
                break;
            case 3: //healer
                E.hp = 7;
                E.speed = 1.6f;
                E.GoldDrop = 4;
                E.tag = "EnnemyHealer";
                break;
            case 4: //sprinter
                E.hp = 8;
                E.speed = 4.8f;
                E.GoldDrop = 5;
                E.tag = "Sprinter";
                break;
            case 5: //flying
                E.hp = 8;
                E.speed = 1.2f;
                E.GoldDrop = 8;
                E.tag = "Flying";
                break;
        }
    }
}
