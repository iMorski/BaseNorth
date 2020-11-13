using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMove : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Image Plane;
    
    public delegate void OnСlick();
    public static event OnСlick Сlick;

    private GameObject Character;

    private bool IsOn;

    private void Awake()
    {
        Сlick += OnButtonMoveClick;
    }

    private void Start()
    {
        GameController.EnemySpawn += OnEnemySpawn;
        GameController.EnemyMove += OnEnemyMove;
        
        ButtonChoose.Сlick += OnButtonChooseClick;
        ButtonSelect.Сlick += OnButtonSelectClick;
    }

    private void OnEnemySpawn(Vector3 SpawnPosition)
    {
        if (IsOn && !(gameObject.transform.position != SpawnPosition))
        {
            Disable();

            IsOn = false;
        }
    }
    
    private void OnEnemyMove(Vector3 MoveCurrentPosition, Vector3 MoveTargetPosition)
    {
        Vector3 Position = gameObject.transform.position;
        
        if (!(Position != MoveCurrentPosition))
        {
            Enable();
            
            IsOn = true;
        }
        else if (!(Position != MoveTargetPosition))
        {
            Disable();

            IsOn = false;
        }
    }
    
    private void OnButtonChooseClick(GameObject ChooseButton, GameObject ChooseCharacter)
    {
        if (IsOn)
        {
            Enable();

            Character = ChooseCharacter;
        }
    }
    
    private void OnButtonSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (IsOn)
        {
            Enable();

            Character = SelectCharacter;
        }
    }
    
    private void OnButtonMoveClick()
    {
        Disable();
    }

    public void OnClick()
    {
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();

        foreach (string CharacterInGameByName in GameController.CharacterInGameByName)
        {
            if (!(Character.name != CharacterInGameByName))
            {
                FB.MyData[Character.name.Replace("Ally-", "")] = $"{Position01} : {Position02}";
                FB.SetValue();
                
                Сlick();
                
                return;
            }
        }
        
        int Count = 0;

        foreach (KeyValuePair<string, string> Data in FB.MyData)
        {
            if (Data.Key.Contains(Character.name))
            {
                Count++;
            }
        }
        
        FB.MyData[$"Character-{Character.name}-0{Count + 1}"] = $"{Position01} : {Position02}";
        FB.SetValue();

        Сlick();
    }

    private void Enable()
    {
        Button.enabled = true;
        Plane.enabled = true;
    }
    
    private void Disable()
    {
        Button.enabled = false;
        Plane.enabled = false;
    }
}
