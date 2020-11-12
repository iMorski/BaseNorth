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
        GameController.EnemyCharacterMove += OnEnemyCharacterMove;
        CharacterSelector.Сlick += OnCharacterSelectorClick;
    }

    private void OnCellClick()
    {
        Vector3 CharacterPosition = Character.transform.position;
        Vector3 CellPosition = gameObject.transform.position;
        
        if (!(CharacterPosition != CellPosition) && Animator.GetBool("IsBusy"))
        {
            Animator.Play("Cell-Plane-Busy-Disable");
            Animator.SetBool("IsBusy", false);
        }
        else if (!Animator.GetBool("IsBusy"))
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
            Animator.Play("Cell-Plane-Busy-Enable");
        }
    }

    private void OnEnemyCharacterMove(Vector3 CharacterCurrentPosition, Vector3 CharacterTargetPosition)
    {
        Vector3 Position = gameObject.transform.position;

        if (!(Position != CharacterCurrentPosition))
        {
            
        }
        else if (!(Position != CharacterTargetPosition))
        {
            
        }
    }
    
    private void OnCharacterSelectorClick(GameObject CharacterSelectorButton)
    {
        if (!Animator.GetBool("IsBusy"))
        {
            Animator.Play("Cell-Plane-Enable");
        }

        Character = CharacterSelectorButton.transform.parent.gameObject;
    }

    public void OnClick()
    {
        Animator.SetBool("IsBusy", true);
        Animator.Play("Cell-Plane-Disable");
        
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();

        foreach (string CharacterByName in GameController.CharacterInGameByName)
        {
            if (!(Character.name != CharacterByName))
            {
                FB.MyData[Character.name.Replace("Ally-", "")] = $"{Position01} : {Position02}";
                FB.SetValue();
                
                Сlick();
                
                Debug.Log($"Moving Character: {Character.name}");
                
                return;
            }
        }
        
        int Count = 0;

        foreach (KeyValuePair<string, string> Data in FB.MyData)
        {
            if (Data.Key.Contains(Character.name))
            {
                Count++;
            }
        }
        
        FB.MyData[$"Character-{Character.name}-0{Count + 1}"] = $"{Position01} : {Position02}";
        FB.SetValue();

        Сlick();
        
        Debug.Log($"Spawning Character: Character-{Character.name}-0{Count + 1}");
    }
}
