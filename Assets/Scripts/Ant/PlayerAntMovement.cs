using UnityEngine;
using System.Collections;

public class PlayerAntMovement : MonoBehaviour {

    public float moveSpeed;
    public float turnSpeed;

    public bool canMove;
    private int layerMask;
    private float collisionZone = 1.5f;


    // Use this for initialization
    void Start () {
        canMove = false;

        //Marcamos para ignorar o layermask onde as Ants NPC estao
        //Dessa forma ira colidir com tudo menos o layer 8
        layerMask = 1 << 8;
        //entao fazemos
        layerMask = ~layerMask;
    }

  
    // Update is called once per frame
    void Update () {
        RaycastHit [] rayHit = new RaycastHit[40];
        int stepAngle = -40;

        for (int j = 0; j < rayHit.Length; j++)
        {
            Physics.Raycast(transform.position, Quaternion.Euler(0, stepAngle, 0) * transform.forward, out rayHit [j], 4f, layerMask);
            stepAngle = stepAngle + 2;
        }
        for (int k = 0; k < rayHit.Length; k++)
            Debug.DrawLine(transform.position, rayHit [k].point);

        canMove = true;

        for (int i = 0; i < rayHit.Length; i++)
        {

            if(i < (rayHit.Length/2 - 10) && i > (rayHit.Length/2 + 10))
            {
                collisionZone = 1;
            }
            if (rayHit [i].transform != null)
            {
                if ((rayHit [i].transform.position - transform.position).magnitude < collisionZone)
                {
                    canMove = false;
                    break;
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && canMove)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

}
