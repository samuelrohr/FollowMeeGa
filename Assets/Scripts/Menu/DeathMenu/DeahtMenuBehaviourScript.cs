using UnityEngine;
using System.Collections;

public class DeahtMenuBehaviourScript : MenuBehaviour {
	

	public override void TreatClickEvent (string buttonName, Transform button) {
		switch (buttonName) {
		case "retryButton":
			Application.LoadLevel("SRScene");
			break;
		case "mainMenuButton":
			Application.Quit();
			break;
		}
	}

}
