using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuantdFormigas : MonoBehaviour 
{
	static List<GameObject> lostAnts = new List<GameObject> ();
	static List<GameObject> foundAnts = new List<GameObject> ();
	Text text;                      
	
	
	void Start ()
	{
		
		text = GetComponent <Text> ();
		
	}
	
	
	void Update ()
	{
		text.text = foundAnts.Count + "             " + lostAnts.Count;
        if(lostAnts.Count + foundAnts.Count == 1)
        {
            Application.LoadLevel("GameOver");
        }
	}

	public static void AddFoundAnt (GameObject ant) {
		if (!foundAnts.Contains(ant)) {
			RemoveLostAnt(ant);
			foundAnts.Add(ant);
		}
	}

	public static void AddLostAnt (GameObject ant) {
		if (!lostAnts.Contains (ant)) {
			lostAnts.Add (ant);
		}
	}

	static void RemoveLostAnt (GameObject ant) {
		if (lostAnts.Contains (ant)) {
			lostAnts.Remove(ant);
		}
	}

	static void RemoveFoundAnt (GameObject ant) {
		if (foundAnts.Contains (ant)) {
			foundAnts.Remove(ant);
		}
	}

	public static void RemoveAnt (GameObject ant) {
		RemoveLostAnt (ant);
		RemoveFoundAnt (ant);
	}
}
