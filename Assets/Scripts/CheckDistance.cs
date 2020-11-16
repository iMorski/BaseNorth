using System.Collections.Generic;
using UnityEngine;

public class CheckDistance : MonoBehaviour
{
    private GameObject Character;
    
    public List<GameObject> EnemyInDistance = new List<GameObject>();

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

    private void OnTriggerStay(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy")||
            Character.name.Contains("Enemy") && other.gameObject.name.Contains("Ally"))
        {
            if (!EnemyInDistance.Contains(other.gameObject))
            {
                EnemyInDistance.Add(other.gameObject);
            }
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
