using UnityEngine;
using UnityEngine.UI;

public class UiCar : MonoBehaviour
{
    public Image Icon;
    
    [SerializeField] private Color ColorAlly;
    [SerializeField] private Color ColorEnemy;

    private void Start()
    {
        GameController.CarMove += OnCarMove;
    }

    private void OnCarMove(Vector3 CurrentPosition, Vector3 NextPosition)
    {
        switch (GameController.MyPosition)
        {
            case 1:

                if (CurrentPosition.x > NextPosition.x || CurrentPosition.z > NextPosition.z)
                {
                    Icon.color = ColorAlly;
                }
                else
                {
                    Icon.color = ColorEnemy;
                }

                break;
            
            case 2:
                
                if (CurrentPosition.x < NextPosition.x || CurrentPosition.z < NextPosition.z)
                {
                    Icon.color = ColorAlly;
                }
                else
                {
                    Icon.color = ColorEnemy;
                }

                break;
        }
    }
}
