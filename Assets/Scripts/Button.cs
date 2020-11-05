using UnityEngine;

public class Button : MonoBehaviour
{
    public void OnClick()
    {
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();
        
        FirebaseController.MyData["Position"] = $"{Position01} : {Position02}";
    }
}
