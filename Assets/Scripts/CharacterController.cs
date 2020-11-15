using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Range(0, 100)][SerializeField] private int Health;
    [Range(0, 100)][SerializeField] private int Damage;
    [Range(0.0f, 1.0f)][SerializeField] private float PositionSmoothValue;
    [Range(0.0f, 1.0f)][SerializeField] private float RotationSmoothValue;
    
    private Button Button;
    private Animator Animator;

    private void Awake()
    {
        Button = GetComponentInChildren<ButtonSelect>().Button.GetComponent<Button>();
        Animator = GetComponentInChildren<Animator>();
    }

    public void SetPath(Vector3 Position)
    {
        Vector3 CurrentPosition = transform.position;
        
        Button.enabled = false;
        Animator.CrossFade("Gun-1H-Run", 0.25f);

        StartCoroutine(FollowPath(Position));
        StartCoroutine(Rotate(CurrentPosition, Position));
    }

    public void BreakPath(GameObject Enemy)
    {
        StopAllCoroutines();
        
        Animator.CrossFade("Gun-1H-Fire", 0.25f);
        Button.enabled = true;
        
        
    }

    private IEnumerator FollowPath(Vector3 Position)
    {
        while (transform.position != Position)
        {
            float Step = (PositionSmoothValue * 10) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Position, Step);
        
            yield return new WaitForEndOfFrame();
        }
        
        Animator.CrossFade("Gun-1H-Wait", 0.25f);
        Button.enabled = true;
    }

    private IEnumerator Rotate(Vector3 CurrentPosition, Vector3 NextPosition)
    {
        Quaternion Rotation = Quaternion.LookRotation(NextPosition - CurrentPosition);

        while (transform.rotation != Rotation)
        {
            float Step = (RotationSmoothValue * 1000) * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, Step);
            
            yield return new WaitForEndOfFrame();
        }
    }
}
