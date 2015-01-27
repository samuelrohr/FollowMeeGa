using UnityEngine;
using System.Collections;

public abstract class MenuBehaviour : MonoBehaviour {
	abstract public void TreatClickEvent(string buttonName, Transform button);
}
