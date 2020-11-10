using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Animator Animator;
    
    public delegate void OnСlick();
    public static event OnСlick Сlick;

    private GameObject Character;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        
        Сlick += OnCellClick;
    }

    private void Start()
    {
        ChooseFighterButton.Сlick += OnChooseFighterButtonClick;
        GameController.EnemyCharacterSpawn += OnEnemyCharacterSpawn;
    }

    private void OnCellClick()
    {
        if (!Animator.GetBool("IsBusy"))
        {
            Animator.Play("Cell-Plane-Disable");
        }
    }

    private void OnChooseFighterButtonClick(string ChooseFighterButtonName, GameObject ChooseFighterButtonCharacter)
    {
        if (!Animator.GetBool("IsBusy"))
        {
            Animator.Play("Cell-Plane-Enable");
        }

        Character = ChooseFighterButtonCharacter;
    }

    private void OnEnemyCharacterSpawn(Vector3 CharacterPosition)
    {
        if (!(CharacterPosition != gameObject.transform.position))
        {
            Animator.SetBool("IsBusy", true);
            Animator.Play("Cell-Plane-Disable");
        }
    }

    public void OnClick()
    {
        Animator.SetBool("IsBusy", true);
        Animator.Play("Cell-Plane-Disable");
        
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();

        int Count = 0;

        foreach (KeyValuePair<string, string> Data in FB.MyData)
        {
            if (Data.Key.Contains(Character.name))
            {
                Count++;
            }
        }
        
        FB.MyData[$"{Character.name}-0{Count + 1}"] = $"{Position01} : {Position02}";
        FB.SetValue();

        Сlick();
    }
}
