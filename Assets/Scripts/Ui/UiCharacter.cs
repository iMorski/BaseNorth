using System;
using UnityEngine;
using UnityEngine.UI;

public class UiCharacter : MonoBehaviour
{
    public Button Button;
    public Image Plane;
    public Image Pointer;
    
    public delegate void OnСlick(Button Button, GameObject Character);
    public static event OnСlick Сlick;

    private GameObject Parent;
    
    private void Awake()
    {
        Сlick += OnCharacterClick;
    }

    private void Start()
    {
        transform.rotation = CameraRotation.RotationQuaternion;

        Parent = transform.parent.parent.gameObject;
        
        if (Parent.name.Contains("Ally"))
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
        
        Сlick(Button, Parent);
    }

    private void OnDestroy()
    {
        UiCell.Сlick -= OnCellClick;
        UiCharacter.Сlick -= OnCharacterClick;
    }
}
