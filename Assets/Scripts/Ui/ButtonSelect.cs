using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public Button Button;
    public Image Plane;
    public Image Pointer;
    
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
        Button.enabled = true;
        Plane.enabled = true;
    }
    
    private void DisableButton()
    {
        Button.enabled = false;
        Plane.enabled = false;
    }
    
    private void EnablePointer()
    {
        Pointer.enabled = true;
    }
    
    private void DisablePointer()
    {
        Pointer.enabled = false;
    }
}
