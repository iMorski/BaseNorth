using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void OnClick()
    {
        string Position01 = Mathf.Round(gameObject.transform.position.x).ToString();
        string Position02 = Mathf.Round(gameObject.transform.position.z).ToString();
        
        FB.MyData["Position"] = $"{Position01} : {Position02}";
        
        if (GameController.IsDeveloper)
        {
            foreach (KeyValuePair<string, string> Data in FB.MyData)
            {
                if (!(Data.Key != "Position"))
                {
                    //Character01.SetPoint(Data.Value);
                }
            }
        }
    }
}
