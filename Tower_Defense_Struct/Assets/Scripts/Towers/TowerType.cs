using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerType : MonoBehaviour
{
    public static TowerType Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetType(GameTileScript Turret)
    {
        switch (TowerManager.Singleton.CurrentTowerIndex)
        {
            case 0: //classic
                Turret.tag = "TurretA";
                break;
            case 1://sniper
                Turret.TurretDamage = 8;
                Turret.Range = 7;
                Turret.AttackSpeed = 2.5f;
                Turret.tag = "TurretB";
                break;
            case 2: //amp
                Turret.TurretDamage = 0;
                Turret.Range = 3;
                Turret.AttackSpeed = 0;
                Turret.tag = "TurretC";
                break;
            case 3: //freeze
                Turret.TurretDamage = 1;
                Turret.Range = 2;
                Turret.AttackSpeed = 2f;
                Turret.tag = "TurretD";
                break;
            case 4: //rocket
                Turret.TurretDamage = 5;
                Turret.Range = 5;
                Turret.AttackSpeed = 2.75f;
                Turret.tag = "TurretE";
                break;

        }
    }
}
