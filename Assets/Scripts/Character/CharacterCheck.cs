using System.Collections.Generic;
using UnityEngine;

public class CharacterCheck : MonoBehaviour
{
    public List<GameObject> Enemy = new List<GameObject>();

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
