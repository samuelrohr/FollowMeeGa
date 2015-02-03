using UnityEngine;
using System.Collections;

public class WinMenuBehavior : MenuBehaviour {

    public override void TreatClickEvent(string buttonName, Transform button){
        switch(buttonName){
            case "next":
                Application.LoadLevel("Match");
                break;
            case "quit":
                Application.Quit();
                break;
        }
    }
}
