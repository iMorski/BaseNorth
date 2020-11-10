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

    private void Update()
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
