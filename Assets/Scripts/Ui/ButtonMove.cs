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
    private bool IsOccupied;

    private void Awake()
    {
        Сlick += OnButtonMoveClick;
    }

    private void Start()
    {
        GameController.Spawn += OnSpawn;
        GameController.Move += OnMove;
        
        ButtonChoose.Сlick += OnButtonChooseClick;
        ButtonSelect.Сlick += OnButtonSelectClick;
    }

    private void OnSpawn(Vector3 SpawnPosition)
    {
        Vector3 Position = gameObject.transform.position;
        
        if (!(Position != SpawnPosition))
        {
            Disable();
            
            IsOccupied = true;
        }
    }
    
    private void OnMove(Vector3 MoveCurrentPosition, Vector3 MoveNextPosition)
    {
        Vector3 Position = gameObject.transform.position;
        
        if (!(Position != MoveCurrentPosition))
        {
            if (IsOn)
            {
                Enable();
            }
            
            IsOccupied = false;
        }
        else if (!(Position != MoveNextPosition))
        {
            Disable();
            
            IsOccupied = true;
        }
    }
    
    private void OnButtonChooseClick(GameObject ChooseButton, GameObject ChooseCharacter)
    {
        if (!IsOccupied)
        {
            Enable();

            Character = ChooseCharacter;
        }

        IsOn = true;
    }
    
    private void OnButtonSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (!IsOccupied)
        {
            Enable();

            Character = SelectCharacter;
        }
        
        IsOn = true;
    }
    
    private void OnButtonMoveClick()
    {
        Disable();
        
        IsOn = false;
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
