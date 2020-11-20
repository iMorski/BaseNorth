using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterContainer;
    
    public delegate void OnSpawn(Vector3 Position);
    public static event OnSpawn Spawn;

    public delegate void OnMove(Vector3 CurrentPosition, Vector3 TargetPosition);
    public static event OnMove Move;
    
    public delegate void OnAttack(GameObject Character, int Health);
    public static event OnAttack Hit;

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
                        if (!(RoomData.Value != FB.MyName) && !($"Ally-{Data.Key}" != CharacterInGameByName[i]) ||
                            RoomData.Value != FB.MyName && !($"Enemy-{Data.Key}" != CharacterInGameByName[i]))
                        {
                            MoveCharacter(i, Data.Value);
                            HitCharacter(i, Data.Value);
                        }
                    }

                    if (!(RoomData.Value != FB.MyName) && !CharacterInGameByName.Contains($"Ally-{Data.Key}"))
                    {
                        SpawnCharacter(Data.Key, Data.Value, "Ally");
                    }
                    else if (RoomData.Value != FB.MyName && !CharacterInGameByName.Contains($"Enemy-{Data.Key}"))
                    {
                        SpawnCharacter(Data.Key, Data.Value, "Enemy");
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

        for (int i = Name.Length - 1; i > 0; i--)
        {
            if (!(Name[i].ToString() != "-"))
            {
                Name = Name.Remove(i, Name.Length - i);

                break;
            }
        }
        
        return CharacterContainer[CharacterContainerByName.IndexOf(Name)];
    }

    private Vector3 CalculatePosition(string Raw)
    {
        Vector3 Position = new Vector3();

        int StatSeparator = Raw.IndexOf(";");
        int PositionSeparator = Raw.IndexOf(":");

        Position.x = int.Parse(Raw.Substring(StatSeparator + 1, (PositionSeparator - 1) - (StatSeparator + 1)));
        Position.z = int.Parse(Raw.Substring(PositionSeparator + 1, Raw.Length - (PositionSeparator + 1)));

        return Position;
    }

    private void SpawnCharacter(string Name, string Position, string Faction)
    {
        GameObject Character = Instantiate(GetCharacterByName(Name), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        
        Character.name = $"{Faction}-{Name}";
                        
        CharacterInGame.Add(Character);
        CharacterInGameByName.Add(Character.name);

        Character = Character.transform.GetChild(0).gameObject;
        Character.transform.position = CalculatePosition(Position);

        Spawn(Character.transform.position);
    }

    private void MoveCharacter(int i, string Position)
    {
        if (CharacterInGame[i].transform.Find("Character") != null)
        {
            GameObject Character = CharacterInGame[i].transform.GetChild(0).gameObject;
        
            Vector3 CurrentPosition = Character.transform.position;
            Vector3 NextPosition = CalculatePosition(Position);
                            
            if (CurrentPosition != NextPosition)
            {
                Character.GetComponent<CharacterController>().SetPosition(NextPosition);

                Move(CurrentPosition, NextPosition);
            }
        }
    }

    private void HitCharacter(int i, string Health)
    {
        if (CharacterInGame[i].transform.Find("Character") != null)
        {
            GameObject Character = CharacterInGame[i].transform.GetChild(0).gameObject;
        
            int CharacterHealth = Character.GetComponent<CharacterController>().Health;
            int DataHealth = int.Parse(Health.Substring(0, Health.IndexOf(" ")));

            if (CharacterHealth != DataHealth)
            {
                Hit(Character, DataHealth);
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
                FB.Disconnect();
            }
            else
            {
                OnPauseSkip = true;
            }
        }
        
    #endif
}