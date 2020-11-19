using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
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
            Button.enabled = true;
            Plane.enabled = true;
        }

        CharacterMovement.Сlick += OnMoveClick;
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }

    private void OnMoveClick()
    {
        Pointer.enabled = false;
    }

    private void OnSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (gameObject != SelectButton)
        {
            Pointer.enabled = false;
        }
    }
    
    public void OnClick()
    {
        Pointer.enabled = true;
        
        Сlick(gameObject, transform.parent.gameObject);
    }
    
    private void OnDestroy()
    {
        CharacterMovement.Сlick -= OnMoveClick;
        CharacterSelection.Сlick -= OnSelectClick;
    }
}
