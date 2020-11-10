using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private GameObject SelectorButton;
    [SerializeField] private GameObject SelectorPointer;
    
    public delegate void OnСlick(GameObject Selector);
    public static event OnСlick Сlick;
    
    /*
    
    [SerializeField] private Image Triangle;
    [SerializeField] private Image Faction;
    [SerializeField] private Color PlayerColor;
    [SerializeField] private Color EnemyColor;

    public static GameObject SelectCharacter;

    private bool IsPlayerCharacter;
    
    */
    
    private void Awake()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
        
        SelectorButton.SetActive(true);

        Сlick += OnSelectorClick;
    }

    private void OnSelectorClick(GameObject Selector)
    {
        if (gameObject != Selector)
        {
            
        }
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }
    
    public void OnClick()
    {
        Сlick(gameObject);
    }
    
    /*

    private void OnRoomDataChange()
    {
        
        if (!(FB.RoomData.Values.First() != FB.MyName))
        {
            if (!(transform.parent.name != "Character-01") || !(transform.parent.name != "Character-02") || !(transform.parent.name != "Character-03"))
            {
                IsPlayerCharacter = true;
            }
        }
        else
        {
            if (!(transform.parent.name != "Character-04") || !(transform.parent.name != "Character-05") || !(transform.parent.name != "Character-06"))
            {
                IsPlayerCharacter = true;
            }
        }
        
        if (IsPlayerCharacter)
        {
            Faction.color = PlayerColor;
        }
        else
        {
            Faction.color = EnemyColor;
        }
        
        FB.RoomDataChange -= OnRoomDataChange;
    }    

    public void SwitchSelector()
    {
        Triangle.enabled = !Triangle.enabled;
    }

    public void OnClick()
    {
        if (IsPlayerCharacter)
        {
            if (SelectCharacter != null)
            {
                SelectCharacter.GetComponentInChildren<CharacterSelector>().SwitchSelector();
            }
            
            SwitchSelector();
            
            SelectCharacter = gameObject;
        }
        
        Debug.Log("Click!");
    }
    
    */
}
