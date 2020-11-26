using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private static CarController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void SetPosition()
    {
        Instance.StartCoroutine(Move());
        Instance.StartCoroutine(Rotate());
    }

    private static IEnumerator Move()
    {
        
        
        yield return new WaitForEndOfFrame();
    }
    
    private static IEnumerator Rotate()
    {
        
        
        yield return new WaitForEndOfFrame();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name.Contains("Ally"))
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
