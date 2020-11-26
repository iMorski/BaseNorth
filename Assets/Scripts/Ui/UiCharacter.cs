using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiCharacter : MonoBehaviour
{
    [SerializeField] private Image Icon;
    
    [SerializeField] private Sprite Melee;
    [SerializeField] private Sprite Pistol;
    [SerializeField] private Sprite Rifle;
    
    [SerializeField] private Slider HP;
    [SerializeField] private Image HPBar;
    
    [SerializeField] private Color HPAlly;
    [SerializeField] private Color HPEnemy;

    [Range(0.0f, 1.0f)][SerializeField] private float HPSmooth;
    
    public Image Circle;
    public Button Button;

    [SerializeField] private Image Pointer;
    
    [SerializeField] private Color AreaAlly;
    [SerializeField] private Color AreaEnemy;
    [SerializeField] private Image Area;
    
    public delegate void OnСlick(Button Button, GameObject Character);
    public static event OnСlick Сlick;

    private GameObject Parent;
    private CharacterController Controller;
    
    private void Awake()
    {
        Сlick += OnCharacterClick;
    }

    private void Start()
    {
        transform.rotation = CameraRotation.RotationQuaternion;

        Parent = transform.parent.parent.gameObject;
        Controller = Parent.GetComponentInChildren<CharacterController>();

        switch (Controller.Weapon)
        {
            case CharacterController.Type.Melee:

                Icon.sprite = Melee;

                break;
            
            case CharacterController.Type.Pistol:
                
                Icon.sprite = Pistol;

                break;
            
            case CharacterController.Type.Rifle:
                
                Icon.sprite = Rifle;

                break;
        }
        
        if (Parent.name.Contains("Ally"))
        {
            HPBar.color = HPAlly;
            Area.color = AreaAlly;
            
            Button.enabled = true;
            Circle.enabled = true;
        }
        else
        {
            HPBar.color = HPEnemy;
            Area.color = AreaEnemy;
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

    public IEnumerator HealthChange(int HealthPast, int HealthFuture)
    {
        float Health = HealthPast;
        
        while (Health != HealthFuture)
        {
            Health = Mathf.SmoothStep(Health, HealthFuture, HPSmooth);
            HP.value = Health;
            
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDestroy()
    {
        UiCell.Сlick -= OnCellClick;
        UiCharacter.Сlick -= OnCharacterClick;
    }
}
