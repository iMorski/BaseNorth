using UnityEngine;
using UnityEngine.UI;

public class CharacterChoice : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    [SerializeField] private RectTransform Position;
    [SerializeField] private Button Button;
    
    public delegate void OnСlick(GameObject Button, GameObject Character);
    public static event OnСlick Сlick;

    private void Awake()
    {
        Сlick += OnChooseСlick;
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
        
        CharacterSelection.Сlick += OnSelectClick;
        CharacterMovement.Сlick += OnMoveClick;
    }
    
    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            Enable();
        }
    }
    
    private void OnSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        Enable();
    }

    private void OnMoveClick()
    {
        Enable();
    }

    private void OnChooseСlick(GameObject ChooseButton, GameObject ChooseCharacter)
    {
        if (gameObject != ChooseButton)
        {
            Enable();
        }
    }
    
    public void OnClick()
    {
        Disable();

        Сlick(gameObject, Character);
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
