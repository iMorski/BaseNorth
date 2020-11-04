using System;
using System.Collections;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Rigidbody Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetPoint(string NextPosition)
    {
        Vector3 Position = new Vector3();

        for (int i = 0; i < NextPosition.Length; i++)
        {
            if (!(i != 0))
            {
                if (NextPosition[i].ToString() != "-")
                {
                    Position.x = int.Parse(NextPosition[i].ToString());
                }
                else
                {
                    Position.x = -int.Parse(NextPosition[i + 1].ToString());
                }
            }
            else if (!(i != NextPosition.Length - 1))
            {
                if (NextPosition[i - 1].ToString() != "-")
                {
                    Position.z = int.Parse(NextPosition[i].ToString());
                }
                else
                {
                    Position.z = -int.Parse(NextPosition[i].ToString());
                }
            }
        }
        
        StartCoroutine(FollowPath(Position));
    }

    private IEnumerator FollowPath(Vector3 Position)
    {
        while (transform.position != Position)
        {
            Rigidbody.velocity = Position - transform.position;
        
            yield return new WaitForEndOfFrame();
        }
    }
}
