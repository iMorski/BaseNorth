using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool _IsDeveloper;

    public static bool IsDeveloper;

    private int MyPosition;

    private void Awake()
    {
        IsDeveloper = _IsDeveloper;
    }

    private void Start()
    {
        FB.MyData.Add("Position", "0 : 0");
        
        FB.ConnectionStepChange += OnConnectionStepChange;
        FB.RoomDataChange += OnRoomDataChange;
    }

    private void OnConnectionStepChange()
    {
        if (!(FB.ConnectionStep != 1.0))
        {
            FB.Connect();
        }
    }

    private void OnRoomDataChange()
    {
        if (!(MyPosition != 0))
        {
            if (!(FB.RoomData.Values.First() != FB.MyName))
            {
                MyPosition = 1;
            }
            else
            {
                MyPosition = 2;
            }
        }
        
        foreach (KeyValuePair<Dictionary<string, string>, string> RoomData in FB.RoomData)
        {
            if (!(RoomData.Value != FB.MyName))
            {
                foreach (KeyValuePair<string, string> Data in RoomData.Key)
                {
                    if (!(Data.Key != "Position"))
                    {
                        if (!(Data.Value != FB.MyData["Position"]))
                        {
                            //Character01.SetPoint(Data.Value);
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> Data in RoomData.Key)
                {
                    if (!(Data.Key != "Position"))
                    {
                        //Character02.SetPoint(Data.Value);
                    }
                }
            }
        }
    }

    #if UNITY_EDITOR

        private void OnApplicationQuit()
        {
            FB.Disconnect();
        }

    #else
    
        bool OnPauseSkip;
    
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
