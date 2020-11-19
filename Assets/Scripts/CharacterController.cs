using System;
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
    private CharacterSelection CharacterSelection;
    
    private Vector3 OnCellPosition;

    private void Awake()
    {
        Die += OnDeath;
        
        CharacterCheck = GetComponentInChildren<CharacterCheck>();
        CharacterAnimator = GetComponentInChildren<Animator>();
        CharacterSelection = GetComponentInChildren<CharacterSelection>();
        
        OnCellPosition = transform.position;
    }

    private void Start()
    {
        GameController.Hit += OnHit;

        string Name = gameObject.name;

        if (Name.Contains("Enemy"))
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

                FB.MyData[Character.name.Replace("Ally", "")] = null;
                FB.SetValue();

                GameController.CharacterInGame.Remove(Character);
                GameController.CharacterInGameByName.Remove(Character.name);

                Destroy(Character);
            }
        }
        else if (CharacterCheck.Enemy.Contains(HitCharacter))
        {
            StartCoroutine(RotateOnAttack(HitCharacter));
            
            CharacterAnimator.CrossFade("Gun-1H-Fire", 0.25f);
            
            if (!(HitHealth > 0.0f))
            {
                StopAllCoroutines();
            }
        }
    }

    private void OnDeath(GameObject DeathCharacter, Vector3 DeathPosition)
    {
        if (CharacterCheck.Enemy.Contains(DeathCharacter))
        {
            CharacterCheck.Enemy.Remove(DeathCharacter);
            
            StartCoroutine(Search());
        }
    }
    
    public void SetPosition(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        
        StopAllCoroutines();

        CharacterAnimator.CrossFade("Gun-1H-Run", 0.25f);
        CharacterSelection.Button.enabled = false;

        StartCoroutine(Move(NextPosition));
        StartCoroutine(RotateOnMove(CurrentPosition, NextPosition));
    }
    
    public void Hit(int IncomingDamage)
    {
        string Name = gameObject.name.Replace("Ally-", "");

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
        CharacterSelection.Button.enabled = true;

        string Name = gameObject.name;

        if (Name.Contains("Enemy"))
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
        while (Enemy != null)
        {
            Vector3 CurrentPosition = transform.position;
            Vector3 NextPosition = Enemy.transform.position;

            Quaternion Rotation = Quaternion.LookRotation(NextPosition - CurrentPosition);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, RotationSmoothValue * 1000 * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator Search()
    {
        GameObject Enemy = new GameObject();
        
        while (!(Enemy != null))
        {
            if (CharacterCheck.Enemy.Any())
            {
                Enemy = CharacterCheck.Enemy[0];
                
                StartCoroutine(Attack(Enemy));
                StartCoroutine(RotateOnAttack(Enemy));
                
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
                string Name = Enemy.name.Replace("Ally-", "");
                string Data = FB.MyData[Name];

                return int.Parse(Data.Substring(0, Data.IndexOf(" ")));
            }

            if (CharacterHealth() > 0.0f)
            {
                Enemy.GetComponent<CharacterController>().Hit(Damage);

                if (!(CharacterHealth() > 0.0f))
                {
                    yield break;
                }
            }

            yield return new WaitForSeconds(Speed);
        }
    }

    private void OnDestroy()
    {
        GameController.Hit -= OnHit;
    }

    /*

    private void OnAttack(GameObject Character, int DataHealth)
    {
        if (!(gameObject != Character))
        {
            Health = DataHealth;
            
            Instantiate(BloodFX, BloodFXSpawnPosition);

            if (!(Health > 0.0f))
            {
                Die(Character, OnCellPosition);

                GameController.Attack -= OnAttack;
                
                CharacterAnimator.CrossFade("Death-01", 0.25f);
                
                StopAllCoroutines();
                StartCoroutine(ToDoDie());

                IEnumerator ToDoDie()
                {
                    GetComponent<CharacterController>().enabled = false;
                    GetComponent<Collider>().enabled = false;

                    transform.Find("Collider").gameObject.SetActive(false);
                    transform.Find("Canvas").gameObject.SetActive(false);
                    
                    IsDead = true;
                    
                    yield return new WaitForSeconds(1.5f);
                    
                    transform.Find("Animator").gameObject.SetActive(false);
                }
            }
        }
        else if (Check.Enemy.Contains(Character))
        {
            Vector3 CurrentPosition = transform.position;
            Vector3 NextPosition = Character.transform.position;

            StartCoroutine(Rotate(CurrentPosition, NextPosition));
            
            CharacterAnimator.CrossFade("Gun-1H-Fire", 0.25f);
        }
    }

    private void OnCharacterDie(GameObject Character, Vector3 Position)
    {
        if (Check.Enemy.Contains(Character))
        {
            Check.Enemy.Remove(Character);

            StartCoroutine(Search());
        }
    }

    public void Reduce(int IncomingDamage)
    {
        string Name = gameObject.name.Replace("Ally-", "");

        FB.MyData[Name] = FB.MyData[Name].Replace(Health.ToString(), (Health - IncomingDamage).ToString());
        FB.SetValue();
    }

    public void SetPosition(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        
        StopAllCoroutines();

        CharacterAnimator.CrossFade("Gun-1H-Run", 0.25f);
        
        Select.ButtonDisable();

        StartCoroutine(Move(NextPosition));
        StartCoroutine(Rotate(CurrentPosition, NextPosition));
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
            int MyDataHealth()
            {
                string Name = Enemy.name.Replace("Ally-", "");
                string Data = FB.MyData[Name];

                return int.Parse(Data.Substring(0, Data.IndexOf(" ")));
            }
            
            if (MyDataHealth() > 0.0f)
            {
                Controller.Reduce(Damage);

                if (!(MyDataHealth() > 0.0f))
                {
                    yield break;
                }
            }

            yield return new WaitForSeconds(Speed);
        }
    }
    
    */
}
