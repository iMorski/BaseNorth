using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float Damage;
    [SerializeField] private float Speed;
    [Range(0.0f, 1.0f)][SerializeField] private float PositionSmoothValue;
    [Range(0.0f, 1.0f)][SerializeField] private float RotationSmoothValue;
    
    public delegate void OnDie(Vector3 Position);
    public static event OnDie Die;

    private Button Button;
    private Animator Animator;
    private CheckDistance CheckDistance;
    private FXController _fxController;

    private Vector3 InGamePosition;

    private void Awake()
    {
        InGamePosition = transform.position;
        
        Button = GetComponentInChildren<ButtonSelect>().Button;
        Animator = GetComponentInChildren<Animator>();
        CheckDistance = GetComponentInChildren<CheckDistance>();
        _fxController = GetComponentInChildren<FXController>();
    }

    public void SetPath(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        InGamePosition = NextPosition;
        
        Button.enabled = false;
        Animator.CrossFade("Gun-1H-Run", 0.25f);
        
        StopAllCoroutines();

        StartCoroutine(FollowPath(NextPosition));
        StartCoroutine(Rotate(CurrentPosition, NextPosition));
    }

    public void TakeDamage()
    {
        _fxController.GetShoot();
        
        if (!(Health > 0.0f))
        {
            StopAllCoroutines();
            
            Animator.CrossFade("Death", 0.25f);

            GameController.CharacterInGame.Remove(gameObject);
            GameController.CharacterInGameByName.Remove(gameObject.name);

            FB.MyData[gameObject.name.Replace("Ally-", "")] = null;
            FB.SetValue();

            Die(InGamePosition);

            StartCoroutine(TakeDown());
        }
    }

    private IEnumerator FollowPath(Vector3 NextPosition)
    {
        while (transform.position != NextPosition)
        {
            float Step = (PositionSmoothValue * 10) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, Step);
        
            yield return new WaitForEndOfFrame();
        }
        
        Animator.CrossFade("Gun-1H-Wait", 0.25f);
        Button.enabled = true;

        StartCoroutine(Search());
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

    private IEnumerator Search()
    {
        GameObject Enemy = null;

        while (!(Enemy != null))
        {
            if (CheckDistance.EnemyInDistance.Any())
            {
                Enemy = CheckDistance.EnemyInDistance[0];
                
                StopCoroutine(Search());
                StartCoroutine(Attack(Enemy));
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator Attack(GameObject Enemy)
    {
        CharacterController EnemyController = Enemy.GetComponent<CharacterController>();
        
        Animator.CrossFade("Gun-1H-Fire", 0.25f);
        
        while (CheckDistance.EnemyInDistance.Contains(Enemy))
        {
            Vector3 CurrentPosition = transform.position;
            Vector3 NextPosition = Enemy.transform.position;

            StartCoroutine(Rotate(CurrentPosition, NextPosition));
            
            EnemyController.Health = EnemyController.Health - Damage;
            EnemyController.TakeDamage();
            
            Debug.Log("Enemy Health: " + EnemyController.Health);

            if (!(EnemyController.Health > 0))
            {
                CheckDistance.EnemyInDistance.Remove(Enemy);
                
                Animator.CrossFade("Gun-1H-Wait", 0.05f);
            }

            yield return new WaitForSeconds(Speed); 
        }
    }

    private IEnumerator TakeDown()
    {
        yield return new WaitForSeconds(1.5f);

        Vector3 CurrentPositon = transform.position;
        Vector3 NextPosition = new Vector3(CurrentPositon.x, -1.5f, CurrentPositon.z);
        
        while (transform.position != NextPosition)
        {
            float Step = (PositionSmoothValue * 10) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, Step);
            
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(gameObject);
    }
}
