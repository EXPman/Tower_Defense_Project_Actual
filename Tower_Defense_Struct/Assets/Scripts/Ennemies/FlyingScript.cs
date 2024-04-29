using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingScript : MonoBehaviour
{
    public Transform TargetTile; // Position de la tuile cible
    public float speed = 1.2f;

    private void Update()
    {
        if (TargetTile != null)
        {
            // Déplace l'ennemi volant vers la cible
            transform.position = Vector3.MoveTowards(transform.position, TargetTile.position, speed * Time.deltaTime);
        }
    }
}
