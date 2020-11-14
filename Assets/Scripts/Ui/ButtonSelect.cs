using UnityEngine;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private GameObject Button;
    [SerializeField] private GameObject Pointer;
    
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
            Button.SetActive(true);
        }

        ButtonMove.Сlick += OnButtonMoveClick;
    }

    private void Update()
    {
        transform.rotation = CameraRotation.RotationQuaternion;
    }

    private void OnButtonMoveClick()
    {
        Pointer.SetActive(false);
    }

    private void OnButtonSelectClick(GameObject SelectButton, GameObject SelectCharacter)
    {
        if (gameObject != SelectButton)
        {
            Pointer.SetActive(false);
        }
    }
    
    public void OnClick()
    {
        Pointer.SetActive(true);
        
        Сlick(gameObject, transform.parent.gameObject);
    }
}
