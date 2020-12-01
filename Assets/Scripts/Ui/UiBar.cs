using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    [SerializeField] private int Min;
    [SerializeField] private int Max;
    [Range(0.0f, 1.0f)][SerializeField] private float _SmoothIncrease;
    [Range(0.0f, 1.0f)][SerializeField] private float _SmoothDecrease;

    public static Slider Slider;

    private static float SmoothIncrease;
    private static float SmoothDecrease;

    private static UiBar Instance;
    
    private static bool IsIncreasing = true;

    private void Awake()
    {
        Instance = this;
        
        SmoothIncrease = _SmoothIncrease;
        SmoothDecrease = _SmoothDecrease;
        
        Slider = GetComponent<Slider>();

        Slider.minValue = Min;
        Slider.maxValue = Max;

        Slider.value = 200;
    }

    private void Start()
    {
        FB.ConnectionStepChange += OnConnectionStepChange;
    }
    
    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 5.0))
        {
            StartCoroutine(Increase());
        }
    }

    public static IEnumerator Increase()
    {
        while (IsIncreasing && Slider.value != Slider.maxValue)
        {
            Slider.value = Slider.value + (SmoothIncrease * 100 * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    public static IEnumerator Decrease(float Value)
    {
        IsIncreasing = false;
        
        while (Slider.value > Value)
        {
            Slider.value = Slider.value - (SmoothDecrease * 10000 * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
        
        IsIncreasing = true;
        
        Instance.StartCoroutine(Increase());
    }
}
