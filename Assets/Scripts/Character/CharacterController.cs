using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private GameObject Team01;
    [SerializeField] private GameObject Team02;
    
    public Type Weapon;

    [Range(0.0f, 1000.0f)] public int Cost;
    
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
    
    public enum Type
    {
        Melee,
        Pistol,
        Rifle
    }

    private void Awake()
    {
        Die += OnDeath;
        
        CharacterCheck = GetComponentInChildren<CharacterCheck>();
        CharacterAnimator = GetComponentInChildren<Animator>();
        CharacterUi = GetComponentInChildren<UiCharacter>();

        switch (GameController.MyPosition)
        {
            case 1:
                
                Team01.SetActive(true);

                break;
            
            case 2:
                
                Team02.SetActive(true);

                break;
        }

        Animation("Wait");
    }

    private void Start()
    {
        GameController.Hit += OnHit;

        if (transform.parent.name.Contains("Enemy"))
        {
            StartCoroutine(Search());
        }
        
        OnCellPosition = transform.position;
    }

    private void OnHit(GameObject HitCharacter, int HitHealth)
    {
        GameObject Character = gameObject;

        if (!(Character != HitCharacter))
        {
            StartCoroutine(CharacterUi.HealthChange(Health, HitHealth));
            
            Health = HitHealth;
            
            Instantiate(BloodFX, BloodFXSpawnPosition);

            if (!(Health > 0.0f))
            {
                UiBar.Slider.value = UiBar.Slider.value + Cost / 2;
                
                Die -= OnDeath;
                Die(Character, OnCellPosition);
                
                Destroy(Character);
            }
        }
        else if (CharacterCheck.Enemy.Contains(HitCharacter))
        {
            StartCoroutine(RotateOnAttack(HitCharacter));
            
            Animation("Attack");
        }
    }

    private void OnDeath(GameObject DeathCharacter, Vector3 DeathPosition)
    {
        if (CharacterCheck.Enemy.Contains(DeathCharacter))
        {
            StopAllCoroutines();
            
            CharacterCheck.Enemy.Remove(DeathCharacter);

            if (transform.parent.name.Contains("Enemy"))
            {
                StartCoroutine(Search());
            }
        }
    }
    
    public void SetPosition(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        
        StopAllCoroutines();

        Animation("Run");

        if (transform.parent.name.Contains("Ally"))
        {
            CharacterUi.Circle.enabled = false;
            CharacterUi.Button.enabled = false;
        }

        StartCoroutine(Move(NextPosition));
        StartCoroutine(RotateOnMove(CurrentPosition, NextPosition));
    }
    
    public void Hit(int IncomingDamage)
    {
        string Name = transform.parent.name.Replace("Ally-", "");

        FB.MyData[Name] = FB.MyData[Name].Replace(Health.ToString(), (Health - IncomingDamage).ToString());
        FB.SetValue();
    }

    private void Animation(string Action)
    {
        switch (Action)
        {
            case "Wait":
                
                switch (Weapon)
                {
                    case Type.Melee:
                
                        CharacterAnimator.CrossFade("Melee-1H-Wait", 0.25f);

                        break;
            
                    case Type.Pistol:
                
                        CharacterAnimator.CrossFade("Gun-1H-Wait", 0.25f);

                        break;
            
                    case Type.Rifle:
                
                        CharacterAnimator.CrossFade("Gun-2H-Wait", 0.25f);

                        break;
                }

                break;
            
            case "Run":
                
                switch (Weapon)
                {
                    case Type.Melee:
                
                        CharacterAnimator.CrossFade("Melee-1H-Run", 0.25f);

                        break;
            
                    case Type.Pistol:
                
                        CharacterAnimator.CrossFade("Gun-1H-Run", 0.25f);

                        break;
            
                    case Type.Rifle:
                
                        CharacterAnimator.CrossFade("Gun-2H-Run", 0.25f);

                        break;
                }

                break;
            
            case "Attack":
                
                switch (Weapon)
                {
                    case Type.Melee:

                        int Random = UnityEngine.Random.Range(0, 3);

                        switch (Random)
                        {
                            case 0:
                            
                                CharacterAnimator.CrossFade("Melee-1H-Attack-01", 0.25f);

                                break;
                        
                            case 1:
                            
                                CharacterAnimator.CrossFade("Melee-1H-Attack-02", 0.25f);

                                break;
                        
                            case 2:
                            
                                CharacterAnimator.CrossFade("Melee-1H-Attack-03", 0.25f);

                                break;
                        }

                        break;
            
                    case Type.Pistol:
                
                        CharacterAnimator.CrossFade("Gun-1H-Fire", 0.25f);

                        break;
            
                    case Type.Rifle:
                
                        CharacterAnimator.CrossFade("Gun-2H-Fire", 0.25f);

                        break;
                }

                break;
        }
    }

    private IEnumerator Move(Vector3 NextPosition)
    {
        OnCellPosition = NextPosition;
        
        while (transform.position != NextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, PositionSmoothValue * 10 * Time.deltaTime);
        
            yield return new WaitForEndOfFrame();
        }
        
        Animation("Wait");

        if (transform.parent.name.Contains("Ally"))
        {
            CharacterUi.Circle.enabled = true;
            CharacterUi.Button.enabled = true;
        }

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
        while (Enemy != null && CharacterCheck.Enemy.Contains(Enemy))
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
        while (Enemy != null && CharacterCheck.Enemy.Contains(Enemy))
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

        if (transform.parent.name.Contains("Enemy"))
        {
            StartCoroutine(Search());
        }
    }

    private void OnDestroy()
    {
        GameController.Hit -= OnHit;
    }
}
