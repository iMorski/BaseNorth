using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterContainer;
    
    public delegate void OnEnemyCharacterSpawn(Vector3 CharacterPosition);
    public static event OnEnemyCharacterSpawn EnemyCharacterSpawn;

    public delegate void OnEnemyCharacterMove(Vector3 CharacterCurrentPosition, Vector3 CharacterTargetPosition);
    public static event OnEnemyCharacterMove EnemyCharacterMove;

    public static List<GameObject> CharacterInGame = new List<GameObject>();
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
                if (Data.Key.Contains("Character"))
                {
                    for (int i = 0; i < CharacterInGameByName.Count; i++)
                    {
                        if (!($"Ally-{Data.Key}" != CharacterInGameByName[i]) || !($"Enemy-{Data.Key}" != CharacterInGameByName[i]))
                        {
                            Movement CharacterInGameMovement = CharacterInGame[i].GetComponent<Movement>();

                            if (!CharacterInGameMovement.Restriction)
                            {
                                Vector3 CurrentPosition = GetCharacterPositionByName(Data.Key);
                                Vector3 TargetPosition = CalculatePosition(Data.Value);
                                
                                StartCoroutine(CharacterInGameMovement.FollowPath(TargetPosition));

                                EnemyCharacterMove(CurrentPosition, TargetPosition);
                            }
                        }
                    }

                    if (!(RoomData.Value != FB.MyName) && !CharacterInGameByName.Contains($"Ally-{Data.Key}"))
                    {
                        string ReplaceCharacter = Data.Key.Replace("Character-", "");
                        string RemoveCharacter = ReplaceCharacter.Remove(ReplaceCharacter.IndexOf("-"), ReplaceCharacter.Length - ReplaceCharacter.IndexOf("-"));
                        
                        GameObject CharacterInContainer = CharacterContainer[CharacterContainerByName.IndexOf(RemoveCharacter)];
                        GameObject Character = Instantiate(CharacterInContainer, CalculatePosition(Data.Value), Quaternion.identity);

                        Character.name = $"Ally-{Data.Key}";
                        
                        CharacterInGame.Add(Character);
                        CharacterInGameByName.Add(Character.name);
                    }
                    else if (RoomData.Value != FB.MyName && !CharacterInGameByName.Contains($"Enemy-{Data.Key}"))
                    {
                        string ReplaceCharacter = Data.Key.Replace("Character-", "");
                        string RemoveCharacter = ReplaceCharacter.Remove(ReplaceCharacter.IndexOf("-"), ReplaceCharacter.Length - ReplaceCharacter.IndexOf("-"));

                        Vector3 Position = CalculatePosition(Data.Value);
                        
                        GameObject CharacterInContainer = CharacterContainer[CharacterContainerByName.IndexOf(RemoveCharacter)];
                        GameObject Character = Instantiate(CharacterInContainer, Position, Quaternion.identity);

                        Character.name = $"Enemy-{Data.Key}";
                        
                        CharacterInGame.Add(Character);
                        CharacterInGameByName.Add(Character.name);
                        
                        EnemyCharacterSpawn(Position);
                    }
                }
            }
        }
    }

    private GameObject GetCharacterByName(string Name)
    {
        if (Name.Contains("Ally"))
        {
            Name = Name.Replace("Ally-", "");
        }
        else if (Name.Contains("Enemy"))
        {
            Name = Name.Replace("Enemy-", "");
        }

        Name = Name.Replace("Character-", "");
        Name = Name.Remove(Name.IndexOf("-"), Name.Length - Name.IndexOf("-"));

        return CharacterContainer[CharacterContainerByName.IndexOf(Name)];
    }

    private Vector3 GetCharacterPositionByName(string Name)
    {
        if (Name.Contains("Ally"))
        {
            Name = Name.Replace("Ally-", "");
        }
        else if (Name.Contains("Enemy"))
        {
            Name = Name.Replace("Enemy-", "");
        }

        Name = Name.Replace("Character-", "");
        Name = Name.Remove(Name.IndexOf("-"), Name.Length - Name.IndexOf("-"));

        return CharacterContainer[CharacterContainerByName.IndexOf(Name)].transform.position;
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