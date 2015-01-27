using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NivelAcucar : MonoBehaviour {

	public static float Acucar = 0f;

	void Start ()
    {
		Coleta (0.00f);
	}


	public static void Coleta(float value)
	{
		Acucar += value;
	}	
}
