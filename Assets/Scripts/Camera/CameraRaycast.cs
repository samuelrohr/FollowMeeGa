using UnityEngine;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

    private RaycastHit [] rayHit = new RaycastHit[40];
    public int angleStep;
    private Transform [] objectList = new Transform[40];

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        angleStep = -80;
        for (int i = 0; i < rayHit.Length; i++)
        {
            Physics.Raycast(transform.position, Quaternion.Euler(0, angleStep, 0) * transform.forward, out rayHit[i]);
            angleStep = angleStep + 2;
        }

        for (int j = 0; j < rayHit.Length; j++)
        {
            Debug.DrawLine(transform.position, rayHit[j].point);
        }

        for (int k = 0; k < rayHit.Length; k++)
        {
            if(rayHit[k].transform != null)
            print(rayHit[k].transform.gameObject.name);
            Transform obj = objectList [k];
            if (obj != null)
            {
                if (!obj.Equals(rayHit [k].transform))
                {
                    obj.gameObject.SetActive(false);
                    rayHit[k].transform.gameObject.SetActive(false); 
                    objectList[k] = rayHit[k].transform;
                }
            } else
            {
                objectList [k] = null;
            }
        }
	}
}
