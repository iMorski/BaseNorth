using System.Collections.Generic;
using UnityEngine;

public class CharacterCheck : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    
    public List<GameObject> Enemy = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        string Name = other.gameObject.GetComponent<CharacterController>().Character.name;
        
        if (Character.name.Contains("Ally") && Name.Contains("Enemy") ||
            Character.name.Contains("Enemy") && Name.Contains("Ally"))
        {
            Enemy.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string Name = other.gameObject.GetComponent<CharacterController>().Character.name;
        
        if (Character.name.Contains("Ally") && Name.Contains("Enemy") ||
            Character.name.Contains("Enemy") && Name.Contains("Ally"))
        {
            Enemy.Remove(other.gameObject);
        }
    }
}
