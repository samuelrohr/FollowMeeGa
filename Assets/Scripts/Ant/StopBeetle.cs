using UnityEngine;
using System.Collections;

public class StopBeetle : MonoBehaviour {
	public float freezeTime;
	bool collidingWithThreat = false;
	GameObject threat = null;

	// Use this for initialization
	void Start () {
	
	}
	

    // Update is called once per frame
    void Update () {
        if (collidingWithThreat && threat != null && Input.GetKeyDown(KeyCode.Space)) {
            GameObject follower = GetComponent<Follow> ().follower;
            if (follower != null) {
                AStarScript.TilePosition currentTile = MapScript.GetTileFromPosition(transform.position);
                BeetleBehavior behavior = threat.GetComponent<BeetleBehavior> ();
                behavior.Freeze(freezeTime);
                GameObject last = follower.GetComponent<SecondaryFollow> ().UnfollowLast();
                if (follower.Equals(last))
                {
                    GetComponent<Follow> ().follower = null;
                }
                NPCAntAStarMovement movementScript = follower.GetComponent<NPCAntAStarMovement> ();
                movementScript.SetNewDestination(currentTile);
            }
        }
    }

	void OnTriggerEnter (Collider other) {
		if (other.tag.Equals ("beetle")) {
			collidingWithThreat = true;
			threat = other.gameObject;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag.Equals ("beetle")) {
			collidingWithThreat = false;
			threat = null;
		}
	}
}
