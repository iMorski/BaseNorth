using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool _IsDeveloper;
    [SerializeField] private CharacterMove Character01;
    [SerializeField] private CharacterMove Character02;
    [SerializeField] private Vector3 StartPosition01;
    [SerializeField] private Vector3 StartPosition02;
    
    private double ConnectionStep;
    
    private Dictionary<string, string> MyData = new Dictionary<string, string>();

    public static bool IsDeveloper;
    public static bool OnDataChange;
    
    private bool OnPauseSkip;

    private int MyPosition;

    private void Awake()
    {
        IsDeveloper = _IsDeveloper;
    }

    private void Start()
    {
        FirebaseController.MyData.Add("Position", "0 : 0");
        MyData = FirebaseController.MyData;
    }

    private void Update()
    {
        if (ConnectionStep != 1.0 && !(FirebaseController.ConnectionStep != 1.0))
        {
            FirebaseController.Connect();
            
            ConnectionStep = 1.0;
        }

        if (ConnectionStep != 5.0 && !(FirebaseController.ConnectionStep != 5.0))
        {
            if (!(FirebaseController.InRoomData.Values.First() != FirebaseController.MyName))
            {
                MyPosition = 1;
                
                FirebaseController.MyData["Position"] = $"{StartPosition01.x} : {StartPosition01.z}";
            }
            else
            {
                MyPosition = 2;

                FirebaseController.MyData["Position"] = $"{StartPosition02.x} : {StartPosition02.z}";
            }
            
            FirebaseController.Write();
            
            ConnectionStep = 5.0;
        }

        if (IsDeveloper)
        {
            foreach (KeyValuePair<string, string> Data in FirebaseController.MyData)
            {
                if (!(Data.Key != "Position"))
                {
                    Character01.SetPoint(Data.Value);
                }
            }
        }
        else
        {
            if (FirebaseController.OnRoomDataChange)
            {
                foreach (KeyValuePair<Dictionary<string, string>, string> InRoomData in FirebaseController.InRoomData)
                {
                    if (!(InRoomData.Value != FirebaseController.MyName))
                    {
                        foreach (KeyValuePair<string, string> Data in InRoomData.Key)
                        {
                            if (!(Data.Key != "Position"))
                            {
                                if (!(Data.Value != FirebaseController.MyData["Position"]))
                                {
                                    Character01.SetPoint(Data.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> Data in InRoomData.Key)
                        {
                            if (!(Data.Key != "Position"))
                            {
                                Character02.SetPoint(Data.Value);
                            }
                        }
                    }
                }
            
                FirebaseController.OnRoomDataChange = false;
            }
        }
    }

    #if UNITY_EDITOR

        private void OnApplicationQuit()
        {
            FirebaseController.Disconnect();
        }

    #else
    
        private void OnApplicationPause(bool OnPause)
        {
            if (OnPause && OnPauseSkip)
            {
                FirebaseController.Disconnect();
            }
            else
            {
                OnPauseSkip = true;
            }
        }
        
    #endif
}
