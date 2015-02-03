using UnityEngine;
using System.Collections;

public class GameOverMenuBehavior : MenuBehaviour {

    public override void TreatClickEvent (string buttonName, Transform button) {
        switch (buttonName) {
            case "retry":
                Application.LoadLevel("Match");
                break;
            case "quit":
                Application.Quit();
                break;
        }
    }
}
