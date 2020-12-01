using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Vector3[] AllyRoute;
    [SerializeField] private Vector3[] EnemyRoute;
    [Range(0.0f, 1.0f)][SerializeField] private float PositionSmoothValue;
    [Range(0.0f, 1.0f)][SerializeField] private float RotationSmoothValue;

    [NonSerialized] public Vector3 OnCellPosition = new Vector3();
    
    private int OnRoutePosition;
    
    private bool OnMove;
    
    private List<string> InDistance = new List<string>();

    public void SetPosition(Vector3 NextPosition)
    {
        StartCoroutine(Move(NextPosition));
        StartCoroutine(Rotate(OnCellPosition, NextPosition));
        
        OnCellPosition = NextPosition;
    }

    private IEnumerator Move(Vector3 NextPosition)
    {
        UiCar UiCar = GetComponentInChildren<UiCar>();
        
        UiCar.Icon.enabled = true;
        
        while (transform.position != NextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, PositionSmoothValue * 10 * Time.deltaTime);
        
            yield return new WaitForEndOfFrame();
        }

        OnMove = false;

        UiCar.Icon.enabled = false;

        StartCoroutine(Check());
    }
    
    private IEnumerator Rotate(Vector3 CurrentPosition, Vector3 NextPosition)
    {
        Quaternion Rotation = Quaternion.LookRotation(NextPosition - CurrentPosition);
            
        while (transform.rotation != Rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, RotationSmoothValue * 1000 * Time.deltaTime);
                
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Check()
    {
        while (!OnMove)
        {
            CheckInDistance();

            yield return new WaitForEndOfFrame();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!(GameController.MyPosition != 1) && (other.transform.parent.name.Contains("Ally") || other.transform.parent.name.Contains("Enemy")))
        {
            InDistance.Add(other.transform.parent.name);

            CheckInDistance();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(GameController.MyPosition != 1) && (other.transform.parent.name.Contains("Ally") || other.transform.parent.name.Contains("Enemy")))
        {
            InDistance.Remove(other.transform.parent.name);

            CheckInDistance();
        }
    }

    private void CheckInDistance()
    {
        if (OnMove || !InDistance.Any()) return;
        
        int CountAlly = 0;
        int CountEnemy = 0;
                
        foreach (string Name in InDistance)
        {
            if (Name.Contains("Ally"))
            {
                CountAlly = CountAlly + 1;
            }
            else if (Name.Contains("Enemy"))
            {
                CountEnemy = CountEnemy + 1;
            }
        }

        if (!(CountEnemy != 0))
        {
            ChangeData("Ally");
        }
        else if (!(CountAlly != 0))
        {
            ChangeData("Enemy");
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
