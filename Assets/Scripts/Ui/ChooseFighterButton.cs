using UnityEngine;

public class ChooseFighterButton : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    
    public delegate void OnСlick(string Name, GameObject Character);
    public static event OnСlick Сlick;

    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        Сlick += OnChooseFighterButtonClick;
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
        Cell.Сlick += OnCellClick;
    }

    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            Animator.Play("ChooseFighterButton-Enable");
        }
    }

    private void OnCellClick()
    {
        Animator.Play("ChooseFighterButton-Enable");
    }

    private void OnChooseFighterButtonClick(string ChooseFighterButtonName, GameObject ChooseFighterButtonCharacter)
    {
        string Name = gameObject.name;
        
        if (ChooseFighterButtonName != Name)
        {
            Animator.Play("ChooseFighterButton-Enable");
        }
    }

    public void OnClick()
    {
        string Name = gameObject.name;
        
        Animator.Play("ChooseFighterButton-Disable");

        Сlick(Name, Character);
    }
}
