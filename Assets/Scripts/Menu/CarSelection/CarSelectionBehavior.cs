using UnityEngine;
using System.Collections;

public class CarSelectionBehavior : MenuBehaviour {
	private int currCarOnDisplay;
	private int totalOfCars;
	private Transform listOfCars;
	private int currPlayer = 0;

	void Start(){
		listOfCars = transform.FindChild("CarsList");
		totalOfCars = listOfCars.childCount;
		currCarOnDisplay = 0;

		//Deixa os demais carros nao visiveis na tela
		for(int i = 1; i < totalOfCars; i++){
			Component[] comp = listOfCars.GetChild(i).GetComponentsInChildren<Renderer>();
			foreach(Renderer renderer in comp){
				renderer.enabled = false;
			}
		}
	}

	public override void TreatClickEvent(string buttonName, Transform button){
		bool checkCarsList = false;
		switch(buttonName){
		case "leftButton":
			currCarOnDisplay = (((currCarOnDisplay - 1) % totalOfCars) + totalOfCars) % totalOfCars;
			checkCarsList = true;
			break;
		case "rightButton":
			currCarOnDisplay = (currCarOnDisplay + 1) % totalOfCars;
			checkCarsList = true;
			break;
		case "okButton":
			if(currPlayer == 0){
				currPlayer = 1;
				ApplicationData.carNamePlayer1 = listOfCars.GetChild(currCarOnDisplay).name;
				transform.FindChild("playerName").GetComponent<TextMesh>().text = "Player2";
			}
			else{
				ApplicationData.carNamePlayer2 = listOfCars.GetChild(currCarOnDisplay).name;
				Application.LoadLevel("LevelSelection");
			}
			break;
		}

		if(checkCarsList){
			Component[] comp;
			for(int i = 0; i < totalOfCars; i++){
				if(currCarOnDisplay == i){
					comp = listOfCars.GetChild(i).GetComponentsInChildren<Renderer>();
					foreach(Renderer renderer in comp){
						renderer.enabled = true;
					}
				}
				else{
					comp = listOfCars.GetChild(i).GetComponentsInChildren<Renderer>();
					foreach(Renderer renderer in comp){
						renderer.enabled = false;
					}
				}
			}
		}
	}
}
