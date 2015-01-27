using UnityEngine;
using System.Collections;

public static class Utilities  {
	public static float getAngle(Vector3 v1, Vector3 v2){
		float n1 = Mathf.Sqrt(v1.x*v1.x + v1.y*v1.y + v1.z*v1.z);
		float n2 = Mathf.Sqrt(v2.x*v2.x + v2.y*v2.y + v2.z*v2.z);
		float aux = v1.x*v2.x + v1.y*v2.y + v1.z*v2.z;	
		float angle = Mathf.Acos(aux/(n1*n2)) * Mathf.Rad2Deg;
		if(angle <= 180)
			return angle;
		else
			return -(360 - angle);
	}
}
