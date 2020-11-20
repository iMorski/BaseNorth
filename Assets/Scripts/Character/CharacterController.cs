using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int Health;
    public int Damage;
    
    [Range(0.0f, 1.0f)][SerializeField] private float Speed;
    [Range(0.0f, 1.0f)][SerializeField] private float PositionSmoothValue;
    [Range(0.0f, 1.0f)][SerializeField] private float RotationSmoothValue;
    [SerializeField] private GameObject BloodFX;
    [SerializeField] private Transform BloodFXSpawnPosition;
    
    public delegate void OnDie(GameObject Character, Vector3 Position);
    public static event OnDie Die;
    
    private CharacterCheck CharacterCheck;
    private Animator CharacterAnimator;
    private UiCharacter CharacterUi;
    
    private Vector3 OnCellPosition;

    private void Awake()
    {
        Die += OnDeath;
        
        CharacterCheck = GetComponentInChildren<CharacterCheck>();
        CharacterAnimator = GetComponentInChildren<Animator>();
        CharacterUi = GetComponentInChildren<UiCharacter>();
        
        OnCellPosition = transform.position;
    }

    private void Start()
    {
        GameController.Hit += OnHit;

        if (transform.parent.name.Contains("Enemy"))
        {
            StartCoroutine(Search());
        }
    }

    private void OnHit(GameObject HitCharacter, int HitHealth)
    {
        GameObject Character = gameObject;

        if (!(Character != HitCharacter))
        {
            Health = HitHealth;
            
            Instantiate(BloodFX, BloodFXSpawnPosition);

            if (!(Health > 0.0f))
            {
                Die -= OnDeath;
                Die(Character, OnCellPosition);
                
                Destroy(Character);
            }
        }
        else if (CharacterCheck.Enemy.Contains(HitCharacter))
        {
            StartCoroutine(RotateOnAttack(HitCharacter));
            
            CharacterAnimator.CrossFade("Gun-1H-Fire", 0.25f);
        }
    }

    private void OnDeath(GameObject DeathCharacter, Vector3 DeathPosition)
    {
        if (CharacterCheck.Enemy.Contains(DeathCharacter))
        {
            StopAllCoroutines();
            
            CharacterCheck.Enemy.Remove(DeathCharacter);
            
            StartCoroutine(Search());
        }
    }
    
    public void SetPosition(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        
        StopAllCoroutines();

        CharacterAnimator.CrossFade("Gun-1H-Run", 0.25f);
        CharacterUi.Button.enabled = false;

        StartCoroutine(Move(NextPosition));
        StartCoroutine(RotateOnMove(CurrentPosition, NextPosition));
    }
    
    public void Hit(int IncomingDamage)
    {
        string Name = transform.parent.name.Replace("Ally-", "");

        FB.MyData[Name] = FB.MyData[Name].Replace(Health.ToString(), (Health - IncomingDamage).ToString());
        FB.SetValue();
    }

    private IEnumerator Move(Vector3 NextPosition)
    {
        OnCellPosition = NextPosition;
        
        while (transform.position != NextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, PositionSmoothValue * 10 * Time.deltaTime);
        
            yield return new WaitForEndOfFrame();
        }
        
        CharacterAnimator.CrossFade("Gun-1H-Wait", 0.25f);
        CharacterUi.Button.enabled = true;

        if (transform.parent.name.Contains("Enemy"))
        {
            StartCoroutine(Search());
        }
    }
    
    private IEnumerator RotateOnMove(Vector3 CurrentPosition, Vector3 NextPosition)
    {
        Quaternion Rotation = Quaternion.LookRotation(NextPosition - CurrentPosition);
            
        while (transform.rotation != Rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, RotationSmoothValue * 1000 * Time.deltaTime);
                
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator RotateOnAttack(GameObject Enemy)
    {
        while (true)
        {
            Vector3 CurrentPosition = transform.position;
            Vector3 NextPosition = Enemy.transform.position;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(NextPosition - CurrentPosition), RotationSmoothValue * 1000 * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator Search()
    {
        while (true)
        {
            if (CharacterCheck.Enemy.Any())
            {
                StartCoroutine(Attack(CharacterCheck.Enemy[0]));
                StartCoroutine(RotateOnAttack(CharacterCheck.Enemy[0]));
                
                yield break;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Attack(GameObject Enemy)
    {
        while (true)
        {
            int CharacterHealth()
            {
                string Name = Enemy.transform.parent.name.Replace("Ally-", "");
                string Data = FB.MyData[Name];

                return int.Parse(Data.Substring(0, Data.IndexOf(" ")));
            }

            yield return new WaitForSeconds(Speed);

            if (CharacterHealth() > 0.0f)
            {
                Enemy.GetComponent<CharacterController>().Hit(Damage);

                if (!(CharacterHealth() > 0.0f))
                {
                    yield break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        GameController.Hit -= OnHit;
    }
}
