using UnityEngine;
using System.Collections;

public class MainMenuBehavior : MenuBehaviour {

	void Start(){
		//Setamos as teclas de comando padrao
		ApplicationData.forwardPlayer1 = KeyCode.W;
		ApplicationData.backwardPlayer1 = KeyCode.S;
		ApplicationData.leftPlayer1 = KeyCode.A;
		ApplicationData.rightPlayer1 = KeyCode.D;

		ApplicationData.forwardPlayer2 = KeyCode.I;
		ApplicationData.backwardPlayer2 = KeyCode.K;
		ApplicationData.leftPlayer2 = KeyCode.J;
		ApplicationData.rightPlayer2 = KeyCode.L;
	}


	public override void TreatClickEvent(string buttonName, Transform button){
		switch(buttonName){
		case "startButton":
			Application.LoadLevel("CarSelection");
			break;
		case "optionsButton":
			//TODO adicionar cena do menu para trocar comandos
			break;
		case "quitButton":
			Application.Quit();
			break;
		}
	}
}
