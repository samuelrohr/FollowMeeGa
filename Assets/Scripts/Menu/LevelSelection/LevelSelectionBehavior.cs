using UnityEngine;
using System.Collections;

public class LevelSelectionBehavior : MenuBehaviour {
	private int currLevelOnDisplay;
	private int totalOfLevel;
	private Transform listOfLevels;
	
	void Start(){
		listOfLevels = transform.FindChild("LevelsList");
		totalOfLevel = listOfLevels.childCount;
		currLevelOnDisplay = 0;
		
		//Deixa os demais carros nao visiveis na tela
		for(int i = 1; i < totalOfLevel; i++){
			listOfLevels.GetChild(i).GetComponent<Terrain>().enabled = false;
	
		}

		ApplicationData.gameMode = "PvP";
	}
	
	public override void TreatClickEvent(string buttonName, Transform button){
		bool checkCarsList = false;
		switch(buttonName){
		case "leftButton":
			currLevelOnDisplay = (((currLevelOnDisplay - 1) % totalOfLevel) + totalOfLevel) % totalOfLevel;
			checkCarsList = true;
			break;
		case "rightButton":
			currLevelOnDisplay = (currLevelOnDisplay + 1) % totalOfLevel;
			checkCarsList = true;
			break;
		case "okButton":
			ApplicationData.levelName = listOfLevels.GetChild(currLevelOnDisplay).name;
			Application.LoadLevel("Match");
			break;
		case "gameMode":
			if(button.GetComponent<TextMesh>().text.Equals("PvP")){
				button.GetComponent<TextMesh>().text = "Derrubada";
				ApplicationData.gameMode = "Derrubada";
			}
			else{
				button.GetComponent<TextMesh>().text = "PvP";
				ApplicationData.gameMode = "PvP";
			}
			break;
		}
		
		if(checkCarsList){
			for(int i = 0; i < totalOfLevel; i++){
				if(currLevelOnDisplay == i){
					listOfLevels.GetChild(i).GetComponent<Terrain>().enabled = true;
				}
				else{
					listOfLevels.GetChild(i).GetComponent<Terrain>().enabled = false;
				}
			}
		}
	}
}
