using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [Range(0.0f, 1.0f)][SerializeField] private float ScrollAngle;
    [Range(0.0f, 1.0f)][SerializeField] private float Smooth;

    public static Quaternion RotationQuaternion;

    private float PreviousAngle;
    private float NextAngle;
    private float Angle;

    private void Start()
    {
        GameController.SetPosition += OnSetPosition;
    }

    private void OnSetPosition()
    {
        switch (GameController.MyPosition)
        {
            case 1:
                
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

                break;
            
            case 2:
                
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                break;
        }
    }

    private void Update()
    {
        if (GameController.MyPosition != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PreviousAngle = Camera.ScreenToViewportPoint(Input.mousePosition).x;
            }
        
            if (Input.GetMouseButton(0))
            {
                NextAngle = Camera.ScreenToViewportPoint(Input.mousePosition).x;
                Angle = (NextAngle - PreviousAngle) * (ScrollAngle * 360);
                PreviousAngle = NextAngle;
            }

            Angle = Mathf.SmoothStep(Angle, 0.0f, Smooth);
            transform.Rotate(0.0f, Angle, 0.0f);

            RotationQuaternion = transform.rotation;
        }
    }
}
