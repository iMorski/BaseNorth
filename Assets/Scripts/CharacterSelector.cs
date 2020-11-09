using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private Image Triangle;
    [SerializeField] private Image Faction;
    [SerializeField] private Color PlayerColor;
    [SerializeField] private Color EnemyColor;

    public static GameObject SelectCharacter;
    
    private Transform CameraTransform;

    private bool IsPlayerCharacter;
    
    private void Awake()
    {
        CameraTransform = GameObject.Find("Camera-Rotation-Point").GetComponent<Transform>();
        
        FB.RoomDataChange += OnRoomDataChange;
    }

    private void Update()
    {
        transform.rotation = CameraTransform.rotation;
    }

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
}
