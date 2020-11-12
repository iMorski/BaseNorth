using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool Restriction;
    
    public IEnumerator FollowPath(Vector3 Position)
    {
        Restriction = true;
            
        while (transform.position != Position)
        {
            transform.position = Vector3.MoveTowards(transform.position, Position, 15.0f * Time.deltaTime);
        
            yield return new WaitForEndOfFrame();
        }

        Restriction = false;
    }
}
