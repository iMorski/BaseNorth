using UnityEngine;
using UnityEngine.UI;

public class UiCell : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Image Plane;
    
    public delegate void OnСlick();
    public static event OnСlick Сlick;

    private GameObject Character;

    private bool IsOccupied;
    private bool IsOn;

    private void Awake()
    {
        Сlick += OnMoveClick;
    }

    private void Start()
    {
        GameController.Spawn += OnSpawn;
        GameController.Move += OnMove;
        
        UiDeck.Сlick += OnDeckClick;
        UiCharacter.Сlick += OnCharacterClick;

        CharacterController.Die += OnDeath;
    }

    private void OnSpawn(Vector3 SpawnPosition)
    {
        if (!(transform.position != SpawnPosition))
        {
            Disable();
            
            IsOccupied = true;
        }
    }
    
    private void OnMove(Vector3 MoveCurrentPosition, Vector3 MoveNextPosition)
    {
        if (!(transform.position != MoveCurrentPosition))
        {
            if (IsOn)
            {
                Enable();
            }
            
            IsOccupied = false;
        }
        else if (!(transform.position != MoveNextPosition))
        {
            Disable();
            
            IsOccupied = true;
        }
    }

    private void OnDeath(GameObject DeathCharacter, Vector3 DeathPosition)
    {
        if (!(transform.position != DeathPosition))
        {
            if (IsOn)
            {
                Enable();
            }
            
            IsOccupied = false;
        }
    }
    
    private void OnDeckClick(Button UiButton, GameObject UiCharacter)
    {
        if (!IsOccupied)
        {
            Enable();

            Character = UiCharacter;
        }

        IsOn = true;
    }
    
    private void OnCharacterClick(Button UiButton, GameObject UiCharacter)
    {
        if (!IsOccupied)
        {
            Enable();

            Character = UiCharacter;
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

        int Health;

        foreach (string Name in GameController.CharacterInGameByName)
        {
            if (!(Character.name != Name))
            {
                string CharacterName = Character.name.Replace("Ally-", "");
                string CharacterData = FB.MyData[CharacterName];

                Health = int.Parse(CharacterData.Substring(0, CharacterData.IndexOf(" ")));

                FB.MyData[Character.name.Replace("Ally-", "")] = $"{Health} ; {Position01} : {Position02}";
                FB.SetValue();
                
                Сlick();
                
                return;
            }
        }

        StartCoroutine(UiBar.Decrease(UiBar.Slider.value - Character.GetComponentInChildren<CharacterController>().Cost));

        int Count = 0;

        foreach (string Name in FB.MyData.Keys)
        {
            if (Name.Contains($"Character-{Character.name}"))
            {
                Count++;
            }
        }
        
        Health = Character.GetComponentInChildren<CharacterController>().Health;
        
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
