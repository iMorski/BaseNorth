using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private GameObject SelectorButton;
    [SerializeField] private GameObject SelectorPointer;
    
    public delegate void OnСlick(GameObject CharacterSelectorButton);
    public static event OnСlick Сlick;
    
    private void Awake()
    {
        Сlick += OnSelectorClick;
    }

    private void Start()
    {
        string ParentName = gameObject.transform.parent.name;
        
        transform.rotation = CameraRotation.RotationQuaternion;
        
        if (ParentName.Contains("Ally"))
        {
            SelectorButton.SetActive(true);
        }
    }

    private void OnSelectorClick(GameObject CharacterSelectorButton)
    {
        if (gameObject != CharacterSelectorButton)
        {
            SelectorPointer.SetActive(false);
        }
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }
    
    public void OnClick()
    {
        SelectorPointer.SetActive(true);
        
        Сlick(gameObject);
    }
}
