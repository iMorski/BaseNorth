using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int Health;

    [SerializeField] private int Damage;
    [SerializeField] private float Speed;
    [Range(0.0f, 1.0f)][SerializeField] private float PositionSmoothValue;
    [Range(0.0f, 1.0f)][SerializeField] private float RotationSmoothValue;
    [SerializeField] private GameObject BloodFX;
    [SerializeField] private Transform BloodFXSpawnPosition;
    
    public delegate void OnDie(Vector3 Position);
    public static event OnDie Die;
    
    private Check Check;
    private Animator Animator;
    private Select Select;

    private void Awake()
    {
        Check = GetComponentInChildren<Check>();
        Animator = GetComponentInChildren<Animator>();
        Select = GetComponentInChildren<Select>();
        
        if (gameObject.name.Contains("Enemy"))
        {
            StartCoroutine(Search());
        }
    }

    private void Start()
    {
        GameController.Attack += OnAttack;
    }

    private void OnAttack(GameObject Character, int InDataHealth)
    {
        if (!(gameObject != Character))
        {
            Health = InDataHealth;
            
            Instantiate(BloodFX, BloodFXSpawnPosition);
        }
        else if (Check.Enemy.Contains(Character))
        {
            Vector3 CurrentPosition = transform.position;
            Vector3 NextPosition = Character.transform.position;

            StartCoroutine(Rotate(CurrentPosition, NextPosition));
            
            Animator.CrossFade("Gun-1H-Fire", 0.25f);
        }
    }

    public void Reduce(int IncomingDamage)
    {
        string Name = gameObject.name.Replace("Ally-", "");
        
        FB.MyData[Name] = FB.MyData[Name].Replace(Health.ToString(), (Health - IncomingDamage).ToString());
        FB.SetValue();
        
        /*
        
        Instantiate(BloodFX, BloodFXSpawnPosition);
        
        if (!(Health > 0.0f))
        {
            Collider.enabled = false;
            
            StopAllCoroutines();

            CharacterAnimator.CrossFade("Death", 0.25f);
            CharacterSelect.UiDisable();

            GameController.CharacterInGame.Remove(gameObject);
            GameController.CharacterInGameByName.Remove(gameObject.name);

            FB.MyData[gameObject.name.Replace("Ally-", "")] = null;
            FB.SetValue();
        }
        
        */
    }

    public void SetPosition(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        
        StopAllCoroutines();

        Animator.CrossFade("Gun-1H-Run", 0.25f);
        
        Select.ButtonDisable();

        StartCoroutine(Move(NextPosition));
        StartCoroutine(Rotate(CurrentPosition, NextPosition));
    }

    private IEnumerator Move(Vector3 NextPosition)
    {
        while (transform.position != NextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, PositionSmoothValue * 10 * Time.deltaTime);
        
            yield return new WaitForEndOfFrame();
        }
        
        Animator.CrossFade("Gun-1H-Wait", 0.25f);
        
        Select.ButtonEnable();

        if (gameObject.name.Contains("Enemy"))
        {
            StartCoroutine(Search());
        }
    }

    private IEnumerator Rotate(Vector3 CurrentPosition, Vector3 NextPosition)
    {
        Quaternion Rotation = Quaternion.LookRotation(NextPosition - CurrentPosition);
            
        while (transform.rotation != Rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, RotationSmoothValue * 1000 * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Search()
    {
        GameObject Enemy = null;

        while (!(Enemy != null))
        {
            if (Check.Enemy.Any())
            {
                Enemy = Check.Enemy[0];
                
                StartCoroutine(GhostAttack(Enemy));
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator GhostAttack(GameObject Enemy)
    {
        CharacterController Controller = Enemy.GetComponent<CharacterController>();
        
        while (Check.Enemy.Contains(Enemy))
        {
            if (Controller.Health > 0.0f)
            {
                Controller.Reduce(Damage);
            }

            yield return new WaitForSeconds(Speed);
        }
    }
}
