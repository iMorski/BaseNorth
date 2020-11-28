using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Vector3[] AllyRoute;
    [SerializeField] private Vector3[] EnemyRoute;

    public static Vector3 Position = new Vector3();
    
    public static bool OnMove;
    
    private static CarController Instance;
    
    private List<string> CharacterInDistanceByName = new List<string>();

    private int OnRoutePosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }

    public static void MoveCar(Vector3 TargetPosition)
    {
        Instance.StartCoroutine(Move(TargetPosition));
        Instance.StartCoroutine(Rotate(Position, TargetPosition));
        
        Position = TargetPosition;
    }

    private static IEnumerator Move(Vector3 TargetPosition)
    {
        yield return new WaitForEndOfFrame();
    }
    
    private static IEnumerator Rotate(Vector3 CurrentPosition, Vector3 TargetPosition)
    {
        /*
        
        Quaternion Rotation = Quaternion.LookRotation(NextPosition - CurrentPosition);
            
        while (transform.rotation != Rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, RotationSmoothValue * 1000 * Time.deltaTime);
                
            yield return new WaitForEndOfFrame();
        }
        
        */
        
        yield return new WaitForEndOfFrame();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!(GameController.MyPosition != 1) && (other.transform.parent.name.Contains("Ally") || other.transform.parent.name.Contains("Enemy")))
        {
            CharacterInDistanceByName.Add(other.transform.parent.name);

            if (!OnMove)
            {
                if (!CharacterInDistanceByName.Contains("Enemy"))
                {
                    ChangeData("Ally");
                }
                else if (!CharacterInDistanceByName.Contains("Ally"))
                {
                    ChangeData("Enemy");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(GameController.MyPosition != 1) && (other.transform.parent.name.Contains("Ally") || other.transform.parent.name.Contains("Enemy")))
        {
            CharacterInDistanceByName.Remove(other.transform.parent.name);

            if (!OnMove && CharacterInDistanceByName.Any())
            {
                if (!CharacterInDistanceByName.Contains("Enemy"))
                {
                    ChangeData("Ally");
                }
                else if (!CharacterInDistanceByName.Contains("Ally"))
                {
                    ChangeData("Enemy");
                }
            }
        }
    }

    private void ChangeData(string Faction)
    {
        OnMove = true;
        
        switch (Faction)
        {
            case "Ally":
                
                OnRoutePosition = OnRoutePosition + 1;

                break;
            
            case "Enemy":
                
                OnRoutePosition = OnRoutePosition - 1;

                break;
        }
        
        if (OnRoutePosition > 0)
        {
            string X = AllyRoute[OnRoutePosition - 1].x.ToString();
            string Z = AllyRoute[OnRoutePosition - 1].z.ToString();
                    
            FB.MyData["Car"] = $"{X} : {Z}";
        }
        else if (OnRoutePosition < 0)
        {
            string X = EnemyRoute[Math.Abs(OnRoutePosition) - 1].x.ToString();
            string Z = EnemyRoute[Math.Abs(OnRoutePosition) - 1].z.ToString();
                    
            FB.MyData["Car"] = $"{X} : {Z}";
        }
        else
        {
            FB.MyData["Car"] = "0 : 0";
        }
                
        FB.SetValue();
    }
}
