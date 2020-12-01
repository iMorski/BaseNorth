using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCheck : MonoBehaviour
{
    public List<GameObject> Enemy = new List<GameObject>();

    private void Start()
    {
        CharacterController.Die += OnDeath;
    }

    private void OnDeath(GameObject Character, Vector3 Position)
    {
        if (Enemy.Contains(Character))
        {
            Enemy.Remove(Character);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && (transform.parent.parent.name.Contains("Ally") && other.transform.parent.name.Contains("Enemy") ||
                                                        transform.parent.parent.name.Contains("Enemy") && other.transform.parent.name.Contains("Ally")))
        {
            Enemy.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character") && (transform.parent.parent.name.Contains("Ally") && other.transform.parent.name.Contains("Enemy") ||
                                              transform.parent.parent.name.Contains("Enemy") && other.transform.parent.name.Contains("Ally")))
        {
            Enemy.Remove(other.gameObject);
        }
    }
}
