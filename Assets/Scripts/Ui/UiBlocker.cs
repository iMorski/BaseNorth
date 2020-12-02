using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiBlocker : MonoBehaviour
{
    [SerializeField] private GameObject Console;
    [SerializeField] private TMP_Text TMP;
    [SerializeField] private Color ColorWin;
    [SerializeField] private Color ColorLoose;
    [SerializeField] private Sprite SpriteWin;
    [SerializeField] private Sprite SpriteLoose;
    
    private Animator Animator;
    private Image Image;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Image = GetComponent<Image>();
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

    public void OnGameOver(string Result)
    {
        switch (Result)
        {
            case "Win":

                TMP.text = "WINNER!\n\nRESTART\nAPPLICATION";
                TMP.color = ColorWin;
                Image.sprite = SpriteWin;


                break;
            
            case "Loose":
                
                TMP.text = "LOOSER :(\n\nRESTART\nAPPLICATION";
                TMP.color = ColorLoose;
                Image.sprite = SpriteLoose;

                break;
        }
        
        Animator.Play("Blocker-Fade-In");
    }

    public void ReStart()
    {
        FB.Disconnect();
    }
}
