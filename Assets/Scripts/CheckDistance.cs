using UnityEngine;

public class CheckDistance : MonoBehaviour
{
    private GameObject Character;
    private CharacterController CharacterController;

    private void Awake()
    {
        Character = transform.parent.gameObject;
        CharacterController = Character.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy"))
        {
            CharacterController.StopAllCoroutines();
        }
        else if (Character.name.Contains("Enemy"))
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Character.name.Contains("Ally") && other.gameObject.name.Contains("Enemy"))
        {
            
        }
        else if (Character.name.Contains("Enemy"))
        {
            
        }
    }
}
