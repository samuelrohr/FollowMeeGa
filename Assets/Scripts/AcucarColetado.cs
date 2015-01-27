using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AcucarColetado : MonoBehaviour
{
       
    public static float metaAcucarfase = 40;
    public Scrollbar nivelAcucar1;

    Text text;

    void Start()
    {
        text = GetComponent <Text>();
    }
    
    void Update()
    {
        nivelAcucar1.size = NivelAcucar.Acucar / AcucarColetado.metaAcucarfase;
        if (NivelAcucar.Acucar <= metaAcucarfase)
        {
            text.text = NivelAcucar.Acucar + "/" + metaAcucarfase;
        } else
            return;


    }
}
