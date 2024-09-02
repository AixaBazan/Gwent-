// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Rendirse : MonoBehaviour
// {
//     private GameObject GM;
//     void Start()
//     {
//         GM = GameObject.Find("GameManager");
//     }

//     public void Click() //Cuando se activa el boton de pasarse
//     {
//         if(GM.GetComponent<GameManager>().IsPlaying == false) //revisa quien se paso
//         {
//             GM.GetComponent<GameManager>().Change1 = true; //No pueden cambiar cartas(en caso de q se pase al inicio del juego)
//             GM.GetComponent<GameManager>().J1CanPlay = true; // el jugador ya no puede jugar mas
//             GM.GetComponent<GameManager>().IsPlaying = true; //cambia el turno
//             Debug.Log("El jugador 1 se rindio");
//         }
//         else
//         {
//             GM.GetComponent<GameManager>().Change2 = true;
//             GM.GetComponent<GameManager>().J2CanPlay = true;
//             GM.GetComponent<GameManager>().IsPlaying = false;
//             Debug.Log("El jugador 2 se rindio");
//         }
//     }
// }
