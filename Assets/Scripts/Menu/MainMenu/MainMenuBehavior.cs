using UnityEngine;
using System.Collections;

public class MainMenuBehavior : MenuBehaviour {

	public override void TreatClickEvent(string buttonName, Transform button){
		switch(buttonName){
		case "start":
			Application.LoadLevel("Match");
			break;
		case "tutorial":
			//TODO adicionar cena do menu para trocar comandos
			break;
		case "quit":
			Application.Quit();
			break;
		}
	}
}
