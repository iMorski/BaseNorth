using UnityEngine;
using UnityEngine.UI;

public class UiCharacter : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    
    public Button Button;
    public Image Plane;
    public Image Pointer;
    
    public delegate void OnСlick(Button Button, GameObject Character);
    public static event OnСlick Сlick;
    
    private void Awake()
    {
        Сlick += OnCharacterClick;
    }

    private void Start()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
        
        if (Character.name.Contains("Ally"))
        {
            Button.enabled = true;
            Plane.enabled = true;
        }

        UiCell.Сlick += OnCellClick;
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }

    private void OnCellClick()
    {
        Pointer.enabled = false;
    }

    private void OnCharacterClick(Button UiButton, GameObject UiCharacter)
    {
        if (Button != UiButton)
        {
            Pointer.enabled = false;
        }
    }
    
    public void OnClick()
    {
        Pointer.enabled = true;
        
        Сlick(Button, Character);
    }
}
