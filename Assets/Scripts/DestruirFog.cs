using UnityEngine;
using System.Collections;

public class DestruirFog : MonoBehaviour {


	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Quad_Black")
		{
			Destroy(col.gameObject);
		}
	}
}
