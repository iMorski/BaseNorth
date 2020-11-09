using System;
using UnityEngine;

public class SummonController : MonoBehaviour
{
    [SerializeField] private Animator[] _SummonAnimator;
    
    private static Animator[] SummonAnimator;

    private void Awake()
    {
        SummonAnimator = _SummonAnimator;
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
    }

    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            foreach (Animator Animator in SummonAnimator)
            {
                Animator.Play("Summon-Enable");
            }
        }
    }

    public static void OnClick(GameObject Character, Animator OnClickAnimator)
    {
        foreach (Animator Animator in SummonAnimator)
        {
            if (!(Animator != OnClickAnimator))
            {
                Animator.Play("Summon-Disable");
            }
            else
            {
                Animator.Play("Summon-Enable");
            }
        }
    }
}
