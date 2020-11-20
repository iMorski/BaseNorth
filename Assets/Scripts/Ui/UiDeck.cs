using UnityEngine;
using UnityEngine.UI;

public class UiDeck : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    [SerializeField] private RectTransform Position;
    [SerializeField] private Button Button;
    
    public delegate void OnСlick(Button Button, GameObject Character);
    public static event OnСlick Сlick;

    private void Awake()
    {
        Сlick += OnDeckСlick;
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
        
        UiCharacter.Сlick += OnCharacterClick;
        UiCell.Сlick += OnCellClick;
    }
    
    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            Enable();
        }
    }
    
    private void OnCharacterClick(Button UiButton, GameObject UiCharacter)
    {
        Enable();
    }

    private void OnCellClick()
    {
        Enable();
    }

    private void OnDeckСlick(Button UiButton, GameObject UiCharacter)
    {
        if (Button != UiButton)
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
    }

    private void Disable()
    {
        Position.anchoredPosition = new Vector2(0.0f, 0.0f);
        Button.enabled = false;
    }
}
