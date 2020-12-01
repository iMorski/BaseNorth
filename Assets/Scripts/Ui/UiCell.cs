using UnityEngine;
using UnityEngine.UI;

public class UiCell : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Image Plane;
    [SerializeField] private Image PlaneBorder;
    [SerializeField] private Color BorderColorAlly;
    [SerializeField] private Color BorderColorEnemy;
    [SerializeField] private Vector3 FinishAlly;
    [SerializeField] private Vector3 FinishEnemy;

    public static Vector3 FinishCell01;
    public static Vector3 FinishCell02;

    public delegate void OnСlick();
    public static event OnСlick Сlick;

    private GameObject Character;

    private bool IsOccupied;
    private bool IsOn;

    private void Awake()
    {
        FinishCell01 = FinishAlly;
        FinishCell02 = FinishEnemy;
        
        Сlick += OnMoveClick;

        if (!(transform.position != new Vector3(0.0f, 0.0f, 0.0f)))
        {
            Disable();
            
            IsOccupied = true;
        }
    }

    private void Start()
    {
        GameController.Spawn += OnSpawn;
        GameController.Move += OnMove;
        GameController.CarMove += OnCarMove;
        GameController.SetPosition += OnSetPosition;
        
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

    private void OnSetPosition()
    {
        switch (GameController.MyPosition)
        {
            case 1:

                if (!(transform.position != FinishAlly))
                {
                    PlaneBorder.color = BorderColorAlly;
                    PlaneBorder.enabled = true;
                }
                else if (!(transform.position != FinishEnemy))
                {
                    PlaneBorder.color = BorderColorEnemy;
                    PlaneBorder.enabled = true;
                }

                break;
            
            case 2:
                
                if (!(transform.position != FinishAlly))
                {
                    PlaneBorder.color = BorderColorEnemy;
                    PlaneBorder.enabled = true;
                }
                else if (!(transform.position != FinishEnemy))
                {
                    PlaneBorder.color = BorderColorAlly;
                    PlaneBorder.enabled = true;
                }

                break;
        }
    }

    private void OnCarMove(Vector3 MoveCurrentPosition, Vector3 MoveNextPosition)
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
            Disable();
            
            switch (GameController.MyPosition)
            {
                case 1:

                    if (transform.position.z < -7.5f)
                    {
                        Enable();
                    }

                    break;
            
                case 2:
                
                    if (transform.position.z > 7.5f)
                    {
                        Enable();
                    }

                    break;
            }
            
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
