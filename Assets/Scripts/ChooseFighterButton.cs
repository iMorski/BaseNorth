using System;
using UnityEngine;

public class ChooseFighterButton : MonoBehaviour
{
    [SerializeField] private GameObject Character;

    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
    }

    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            Animator.Play("ChooseFighterButton-Enable");
        }
    }

    public void OnClick()
    {
        //foreach (Animator Animator in ChooseFighterButtonAnimator)
        {
            //if (!(Animator != OnClickAnimator))
            {
                //Animator.Play("Summon-Disable");
            }
            //else
            {
                //Animator.Play("Summon-Enable");
            }
        }
    }
}
