using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public Button Button;
    public Image Plane;
    public Image Pointer;
    
    public delegate void OnСlick(GameObject Button, GameObject Character);
    public static event OnСlick Сlick;
    
    private void Awake()
    {
        Сlick += OnSelectClick;
    }

    private void Start()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
        
        if (gameObject.transform.parent.name.Contains("Ally"))
        {
            UiEnable();
        }

        Move.Сlick += OnMoveClick;
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }

    private void OnMoveClick()
    {
        PointerDisable();
    }

    private void OnSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (gameObject != SelectButton)
        {
            PointerDisable();
        }
    }
    
    public void OnClick()
    {
        PointerEnable();
        
        Сlick(gameObject, transform.parent.gameObject);
    }

    public void UiEnable()
    {
        Button.enabled = true;
        Plane.enabled = true;
    }
    
    public void UiDisable()
    {
        Button.enabled = false;
        Plane.enabled = false;
    }

    public void ButtonEnable()
    {
        Button.enabled = true;
    }
    
    public void ButtonDisable()
    {
        Button.enabled = false;
    }
    
    public void PointerEnable()
    {
        Pointer.enabled = true;
    }
    
    public void PointerDisable()
    {
        Pointer.enabled = false;
    }
}
