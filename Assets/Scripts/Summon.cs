using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] private GameObject Character;

    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void OnClick()
    {
        SummonController.OnClick(Character, Animator);
    }
}
