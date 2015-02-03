using UnityEngine;
using System.Collections;

public class SugarScript : MonoBehaviour {

    public float totalOfSugar;

    public void Eat()
    {
        totalOfSugar--;
    }

    public bool hasMorePieces()
    {
        if (totalOfSugar > 0)
            return true;
        else
            return false;
    }
}
