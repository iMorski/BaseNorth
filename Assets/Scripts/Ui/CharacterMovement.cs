using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Image Plane;
    
    public delegate void OnСlick();
    public static event OnСlick Сlick;

    private bool IsOn;
    private bool IsOccupied;

    private GameObject Character;

    private void Awake()
    {
        Сlick += OnMoveClick;
    }

    private void Start()
    {
        GameController.Spawn += OnSpawn;
        GameController.Move += OnMove;

        CharacterController.Die += OnDeath;
        CharacterChoice.Сlick += OnChooseClick;
        CharacterSelection.Сlick += OnSelectClick;
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

    private void OnDeath(GameObject DeathCharacter, Vector3 DeathPosition)
    {
        Vector3 Position = gameObject.transform.position;
        
        if (!(Position != DeathPosition))
        {
            if (IsOn)
            {
                Enable();
            }
            
            IsOccupied = false;
        }
    }
    
    private void OnChooseClick(GameObject ChooseButton, GameObject ChooseCharacter)
    {
        if (!IsOccupied)
        {
            Enable();

            Character = ChooseCharacter;
        }

        IsOn = true;
    }
    
    private void OnSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (!IsOccupied)
        {
            Enable();

            Character = SelectCharacter;
        }
        
        IsOn = true;
    }
    
    private void OnMoveClick()
    {
        Disable();
        
        IsOn = false;
    }

    public void OnClick()
    {
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();

        int Health = Character.GetComponent<CharacterController>().Health;

        foreach (string Name in GameController.CharacterInGameByName)
        {
            if (!(Character.name != Name))
            {
                FB.MyData[Character.name.Replace("Ally-", "")] = $"{Health} ; {Position01} : {Position02}";
                FB.SetValue();
                
                Сlick();
                
                return;
            }
        }
        
        int Count = 0;

        foreach (string Name in FB.MyData.Keys)
        {
            if (Name.Contains($"Character-{Character.name}"))
            {
                Count++;
            }
        }
        
        FB.MyData[$"Character-{Character.name}-0{Count + 1}"] = $"{Health} ; {Position01} : {Position02}";
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
