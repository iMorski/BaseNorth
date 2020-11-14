using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChoose : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    [SerializeField] private RectTransform Position;
    [SerializeField] private Button Button;
    
    public delegate void OnСlick(GameObject Button, GameObject Character);
    public static event OnСlick Сlick;

    private void Awake()
    {
        Сlick += OnButtonChooseСlick;
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
        
        ButtonSelect.Сlick += OnButtonSelectClick;
        ButtonMove.Сlick += OnButtonMoveClick;
    }
    
    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            Enable();
        }
    }
    
    private void OnButtonSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        Enable();
    }

    private void OnButtonMoveClick()
    {
        Enable();
    }

    private void OnButtonChooseСlick(GameObject ChooseButton, GameObject ChooseCharacter)
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
