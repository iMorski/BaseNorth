using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public GameObject Button;
    public GameObject Pointer;
    
    public delegate void OnСlick(GameObject Button, GameObject Character);
    public static event OnСlick Сlick;
    
    private void Awake()
    {
        Сlick += OnButtonSelectClick;
    }

    private void Start()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
        
        if (gameObject.transform.parent.name.Contains("Ally"))
        {
            EnableButton();
        }

        ButtonMove.Сlick += OnButtonMoveClick;
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }

    private void OnButtonMoveClick()
    {
        DisablePointer();
    }

    private void OnButtonSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (gameObject != SelectButton)
        {
            DisablePointer();
        }
    }
    
    public void OnClick()
    {
        EnablePointer();
        
        Сlick(gameObject, transform.parent.gameObject);
    }

    private void EnableButton()
    {
        Button.GetComponent<Button>().enabled = true;
        Button.GetComponent<Image>().enabled = true;
    }
    
    private void DisableButton()
    {
        Button.GetComponent<Button>().enabled = false;
        Button.GetComponent<Image>().enabled = false;
    }
    
    private void EnablePointer()
    {
        Pointer.GetComponent<Image>().enabled = true;
    }
    
    private void DisablePointer()
    {
        Pointer.GetComponent<Image>().enabled = false;
    }
}
