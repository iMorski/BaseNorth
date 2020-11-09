using UnityEngine;

public class Cell : MonoBehaviour
{
    public void OnClick()
    {
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();
        
        //FB.MyData[Character] = $"{Position01} : {Position02}";
        FB.SetValue();
    }
}
