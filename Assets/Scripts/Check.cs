using System;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    [NonSerialized] public List<GameObject> Enemy = new List<GameObject>();
    
    private GameObject Character;

    private void Awake()
    {
        Character = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy") ||
            Character.name.Contains("Enemy") && other.gameObject.name.Contains("Ally"))
        {
            Enemy.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy") ||
            Character.name.Contains("Enemy") && other.gameObject.name.Contains("Ally"))
        {
            Enemy.Remove(other.gameObject);
        }
    }
}
