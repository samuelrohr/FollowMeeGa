using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecondaryFollow : MonoBehaviour
{
    public GameObject follower;
    public int frameOffset;
    public bool following = false;
    List<Vector3> positionHistory = new List<Vector3>();
    List<Vector3> angleHistory = new List<Vector3>(); 


    // Use this for initialization
    void Start()
    {

    }
    
    void UpdatePositionAndAngleHistory()
    {
        positionHistory.Insert(0, transform.position);
        angleHistory.Insert(0, transform.eulerAngles);
        if (positionHistory.Count > frameOffset + 1)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
            angleHistory.RemoveAt(angleHistory.Count - 1);
        }
    }

    public void FollowObject(GameObject target)
    {

        if (follower == null)
        {
            SecondaryFollow secFollow = target.GetComponent<SecondaryFollow>();
            secFollow.following = true;
            follower = target;
        } else
        {
            SecondaryFollow secFollow = follower.GetComponent<SecondaryFollow>();
            secFollow.FollowObject(target);
        }
    }
    
    public void UpdateFollower()
    {
        UpdatePositionAndAngleHistory();
        if (follower != null && positionHistory.Count > frameOffset)
        {
            Vector3 position = positionHistory [positionHistory.Count - 1];
            
            Vector3 angle = angleHistory [angleHistory.Count - 1];
            positionHistory.RemoveAt(positionHistory.Count - 1);
            angleHistory.RemoveAt(angleHistory.Count - 1);
            follower.transform.position = position;
            follower.transform.eulerAngles = angle;
            SecondaryFollow secFollow = follower.GetComponent<SecondaryFollow>();
            if (secFollow != null)
                secFollow.UpdateFollower();
            //follower.UpdateFollower();
        } 
    }

    public GameObject UnfollowLast()
    {
        // Checamos se é a última formiga da fila
        if (follower == null)
        {
            // Retorna a si mesma
            following = false;
            return transform.gameObject;
        } else
        {
            // Caso contrário, pedimos para a próxima
            GameObject temp = follower.GetComponent<SecondaryFollow>().UnfollowLast();
            if (follower.Equals(temp))
            {
                follower = null;
            }
            return temp;
        }
    }

	public void Unfollow() {
		following = false;
		if (follower != null) {
			follower.GetComponent<SecondaryFollow>().Unfollow();
		}

		follower = null;
	}


}
