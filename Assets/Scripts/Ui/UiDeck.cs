using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDeck : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    [SerializeField] private Image Icon;

    [SerializeField] private Sprite Melee;
    [SerializeField] private Sprite Pistol;
    [SerializeField] private Sprite Rifle;
    
    [SerializeField] private TMP_Text Price;
    [SerializeField] private RectTransform Position;
    [SerializeField] private Button Button;
    [SerializeField] private GameObject Block;
    
    public delegate void OnСlick(Button Button, GameObject Character);
    public static event OnСlick Сlick;

    private int Cost;

    private bool IsOn;
    private bool IsAvailable;

    private void Awake()
    {
        CharacterController Controller = Character.GetComponentInChildren<CharacterController>();
        
        Cost = Controller.Cost;

        switch (Controller.Weapon)
        {
            case CharacterController.Type.Melee:

                Icon.sprite = Melee;

                break;
            
            case CharacterController.Type.Pistol:
                
                Icon.sprite = Pistol;

                break;
            
            case CharacterController.Type.Rifle:
                
                Icon.sprite = Rifle;

                break;
        }
        
        Price.text = $"${Cost.ToString()}";
        
        Сlick += OnDeckСlick;
    }

    private void Start()
    {
        UiCharacter.Сlick += OnCharacterClick;
        UiCell.Сlick += OnCellClick;
    }

    private void Update()
    {
        if (!IsAvailable && UiBar.Slider.value > Cost)
        {
            IsAvailable = true;
            
            Block.SetActive(false);

            if (!IsOn)
            {
                Enable();
            }
        }
        else if (IsAvailable && UiBar.Slider.value < Cost)
        {
            IsAvailable = false;
            
            Block.SetActive(true);

            if (IsOn)
            {
                Disable();
            }
        }
    }
    
    private void OnCharacterClick(Button UiButton, GameObject UiCharacter)
    {
        if (!IsOn && IsAvailable)
        {
            Enable();
        }
    }

    private void OnCellClick()
    {
        if (!IsOn && IsAvailable)
        {
            Enable();
        }
    }

    private void OnDeckСlick(Button UiButton, GameObject UiCharacter)
    {
        if (!IsOn && IsAvailable && Button != UiButton)
        {
            Enable();
        }
    }
    
    public void OnClick()
    {
        Disable();
        
        Сlick(Button, Character);
    }
    
    private void Enable()
    {
        Position.anchoredPosition = new Vector2(-5.0f, 5.0f);
        Button.enabled = true;

        IsOn = true;
    }

    private void Disable()
    {
        Position.anchoredPosition = new Vector2(0.0f, 0.0f);
        Button.enabled = false;

        IsOn = false;
    }
}
