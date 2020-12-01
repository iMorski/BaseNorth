using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiBlocker : MonoBehaviour
{
    [SerializeField] private GameObject Console;
    [SerializeField] private TMP_Text TMP;
    [SerializeField] private Color ColorWin;
    [SerializeField] private Color ColorLoose;
    
    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameController.SetPosition += OnSetPosition;
        CarController.GameOver += OnGameOver;
    }

    private void OnSetPosition()
    {
        Console.SetActive(false);
        Animator.Play("Blocker-Fade-Out");
    }

    private void OnGameOver(string Result)
    {
        switch (Result)
        {
            case "Win":

                TMP.text = "WINNER!";
                TMP.color = ColorWin;

                break;
            
            case "Loose":
                
                TMP.text = "LOOSER :(";
                TMP.color = ColorLoose;

                break;
        }
        
        Animator.Play("Blocker-Fade-In");
    }

    public void ReStart()
    {
        FB.Disconnect();
    }
}
