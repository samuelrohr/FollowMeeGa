using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follow : MonoBehaviour
{
    public GameObject follower;
    public int frameOffset;
    List<Vector3> positionHistory = new List<Vector3>();
    List<Vector3> angleHistory = new List<Vector3>();
    int flagUpdate = 4;

    private string antNPCTag = ColliderTags.antNPCTag;
    private string sugarTag = ColliderTags.sugarColliderTag;

	bool collidingWithSugar = false;
	GameObject collidingSugar = null;

    // Use this for initialization
    void Start()
    {
        UpdatePositionAndAngleHistory();
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
    
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && flagUpdate >= 4)
        {
            UpdatePositionAndAngleHistory();
            if (follower != null && positionHistory.Count > frameOffset)
            {

                UpdateFollower();
            }
        } else
        {
            flagUpdate++;
        }

		if (collidingWithSugar && collidingSugar != null && Input.GetKeyDown (KeyCode.J) && follower != null) {
			GameObject last = follower.GetComponent<SecondaryFollow> ().UnfollowLast();
			if (follower.Equals(last))
			{
				follower = null;
			}
			last.GetComponent<NPCAntJobScript> ().AssignJob(MapScript.GetTileFromPosition(transform.position), collidingSugar.transform);
		}

    }

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals(sugarTag)) {
			collidingWithSugar = true;
			collidingSugar = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag.Equals(sugarTag)) {
			collidingWithSugar = false;
			collidingSugar = null;
		}
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals(antNPCTag))
        {

            if (!other.GetComponent<NPCAntJobScript>().isWorking)
            {
                SecondaryFollow secFollow = other.gameObject.GetComponent<SecondaryFollow>();
                if (!secFollow.following)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        FollowObject(other.gameObject);
						QuantdFormigas.AddFoundAnt(other.gameObject);
                    }
                }
            }
        }
    }

    void FollowObject(GameObject target)
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

    void OnCollisionEnter()
    {
        flagUpdate = 0;
    }

    void OnCollisionStay()
    {
        flagUpdate = 0;
    }
}
