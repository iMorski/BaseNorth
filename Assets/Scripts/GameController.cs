using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterContainer;
    
    public delegate void OnEnemyCharacterSpawn(Vector3 CharacterPosition);
    public static event OnEnemyCharacterSpawn EnemyCharacterSpawn;

    public static Dictionary<GameObject, Vector3> CharacterInGame = new Dictionary<GameObject, Vector3>();
    public static List<string> CharacterInGameByName = new List<string>();
    
    private List<string> CharacterContainerByName = new List<string>();

    private void Awake()
    {
        foreach (GameObject Character in CharacterContainer)
        {
            CharacterContainerByName.Add(Character.name);
        }
    }

    private void Start()
    {
        FB.MyData.Add("Nick", "");
        
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
        foreach (KeyValuePair<Dictionary<string, string>, string> RoomData in FB.RoomData)
        {
            foreach (KeyValuePair<string, string> Data in RoomData.Key)
            {
                for (int i = 0; i < CharacterContainerByName.Count; i++)
                {
                    if (Data.Key.Contains(CharacterContainerByName[i]))
                    {
                        if (!(RoomData.Value != FB.MyName))
                        {
                            if (!CharacterInGameByName.Contains($"Ally-{Data.Key}"))
                            {
                                Vector3 CharacterPosition = CalculatePosition(Data.Value);
                                GameObject Character = Instantiate(CharacterContainer[i], CharacterPosition, Quaternion.identity);
                                
                                Character.name =  $"Ally-{Data.Key}";
                                
                                CharacterInGame.Add(Character, CharacterPosition);
                                CharacterInGameByName.Add(Character.name);
                            }
                        }
                        else
                        {
                            if (!CharacterInGameByName.Contains($"Enemy-{Data.Key}"))
                            {
                                Vector3 CharacterPosition = CalculatePosition(Data.Value);
                                GameObject Character = Instantiate(CharacterContainer[i], CharacterPosition, Quaternion.identity);
                                
                                Character.name =  $"Enemy-{Data.Key}";
                                
                                CharacterInGame.Add(Character, CharacterPosition);
                                CharacterInGameByName.Add(Character.name);
                                
                                EnemyCharacterSpawn(CharacterPosition);
                            }
                        }
                    }
                }
            }
        }
    }

    private Vector3 CalculatePosition(string Raw)
    {
        Vector3 Position = new Vector3();
        
        for (int i = 0; i < Raw.Length; i++)
        {
            if (!(i != 0))
            {
                if (Raw[i].ToString() != "-")
                {
                    Position.x = int.Parse(Raw[i].ToString());
                }
                else
                {
                    Position.x = -int.Parse(Raw[i + 1].ToString());
                }
            }
            else if (!(i != Raw.Length - 1))
            {
                if (Raw[i - 1].ToString() != "-")
                {
                    Position.z = int.Parse(Raw[i].ToString());
                }
                else
                {
                    Position.z = -int.Parse(Raw[i].ToString());
                }
            }
        }

        return Position;
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
                FB.Disconnect();
            }
            else
            {
                OnPauseSkip = true;
            }
        }
        
    #endif
}