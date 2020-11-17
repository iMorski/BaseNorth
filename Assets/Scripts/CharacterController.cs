using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    private Collider Collider;
    private ColliderSearch ColliderSearch;
    
    private Animator Animator;
    private Select Select;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        ColliderSearch = GetComponentInChildren<ColliderSearch>();
        
        Animator = GetComponentInChildren<Animator>();
        Select = GetComponentInChildren<Select>();

        StartCoroutine(Search());
    }

    public void SetPath(Vector3 NextPosition)
    {
        Vector3 CurrentPosition = transform.position;
        
        StopAllCoroutines();

        Animator.CrossFade("Gun-1H-Run", 0.25f);
        Select.ButtonDisable();

        StartCoroutine(Move(NextPosition));
        StartCoroutine(Rotate(CurrentPosition, NextPosition));
    }

    public void Reduce()
    {
        Instantiate(BloodFX, BloodFXSpawnPosition);
        
        if (!(Health > 0.0f))
        {
            Collider.enabled = false;
            
            StopAllCoroutines();

            Animator.CrossFade("Death", 0.25f);
            Select.UiDisable();

            GameController.CharacterInGame.Remove(gameObject);
            GameController.CharacterInGameByName.Remove(gameObject.name);

            FB.MyData[gameObject.name.Replace("Ally-", "")] = null;
            FB.SetValue();
        }
    }

    private IEnumerator Move(Vector3 NextPosition)
    {
        while (transform.position != NextPosition)
        {
            float Step = (PositionSmoothValue * 10) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, NextPosition, Step);
        
            yield return new WaitForEndOfFrame();
        }
        
        Animator.CrossFade("Gun-1H-Wait", 0.25f);
        Select.ButtonEnable();

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
            if (ColliderSearch.EnemyInDistance.Any())
            {
                Enemy = ColliderSearch.EnemyInDistance[0];
                
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
        
        while (ColliderSearch.EnemyInDistance.Contains(Enemy))
        {
            Vector3 CurrentPosition = transform.position;
            Vector3 NextPosition = Enemy.transform.position;

            StartCoroutine(Rotate(CurrentPosition, NextPosition));

            if (!(EnemyController.Health != 0.0f) || EnemyController.Health < 0.0f)
            {
                Animator.CrossFade("Gun-1H-Wait", 0.0f);
                ColliderSearch.EnemyInDistance.Remove(Enemy);
            }
            else
            {
                EnemyController.Health = EnemyController.Health - Damage;
                EnemyController.Reduce();
                
                Debug.Log("Enemy Health: " + EnemyController.Health);
            }

            yield return new WaitForSeconds(Speed); 
        }
    }
}
