using System;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSearch : MonoBehaviour
{
    private GameObject Character;
    
    [NonSerialized] public List<GameObject> EnemyInDistance = new List<GameObject>();

    private void Awake()
    {
        Character = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy") ||
            Character.name.Contains("Enemy") && other.gameObject.name.Contains("Ally"))
        {
            EnemyInDistance.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy") ||
            Character.name.Contains("Enemy") && other.gameObject.name.Contains("Ally"))
        {
            EnemyInDistance.Remove(other.gameObject);
        }
    }
}
