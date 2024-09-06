using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartelManager : MonoBehaviour
{
    public GameObject cartel;
    public TMP_Text texto;
    void Start()
    {
        cartel.SetActive(false);
    }
    public void MostrarCartel(string mensaje)
    {
        texto.text = mensaje;
        cartel.SetActive(true);
        Invoke("OcultarCartel", 5f);
    }
    void OcultarCartel()
    {
        cartel.SetActive(false);
    }
}
