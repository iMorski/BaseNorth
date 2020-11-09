using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private string RawCurrentPosition;
    
    private bool MovementRestriction;
    
    private void Awake()
    {
        FB.RoomDataChange += OnRoomDataChange;
    }

    private void OnRoomDataChange()
    {
        if (MovementRestriction) return;
        
        foreach (KeyValuePair<Dictionary<string, string>, string> RoomData in FB.RoomData)
        {
            if (!(RoomData.Value != FB.MyName))
            {
                foreach (KeyValuePair<string, string> Data in RoomData.Key)
                {
                    if (!(Data.Key != gameObject.name) && !(Data.Value != FB.MyData[Data.Key]))
                    {
                        GameController.ActionRestriction = false;

                        if (Data.Value != RawCurrentPosition)
                        {
                            RawCurrentPosition = Data.Value;
                            CalculatePath(RawCurrentPosition);
                            
                            MovementRestriction = true;
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> Data in RoomData.Key)
                {
                    if (!(Data.Key != gameObject.name))
                    {
                        if (Data.Value != RawCurrentPosition)
                        {
                            RawCurrentPosition = Data.Value;
                            CalculatePath(RawCurrentPosition);
                            
                            MovementRestriction = true;
                        }
                    }
                }
            }
        }
    }

    private void CalculatePath(string RawString)
    {
        Vector3 NextPosition = new Vector3();

        for (int i = 0; i < RawString.Length; i++)
        {
            if (!(i != 0))
            {
                if (RawString[i].ToString() != "-")
                {
                    NextPosition.x = int.Parse(RawString[i].ToString());
                }
                else
                {
                    NextPosition.x = -int.Parse(RawString[i + 1].ToString());
                }
            }
            else if (!(i != RawString.Length - 1))
            {
                if (RawString[i - 1].ToString() != "-")
                {
                    NextPosition.z = int.Parse(RawString[i].ToString());
                }
                else
                {
                    NextPosition.z = -int.Parse(RawString[i].ToString());
                }
            }
        }
        
        StartCoroutine(FollowPath(NextPosition));
    }

    private IEnumerator FollowPath(Vector3 NextPosition)
    {
        while (transform.position != NextPosition)
        {
            Vector3 CurrentPosition = transform.position;
            float Step = 15.0f * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(CurrentPosition, NextPosition, Step);
        
            yield return new WaitForEndOfFrame();
        }

        MovementRestriction = false;
    }
}
