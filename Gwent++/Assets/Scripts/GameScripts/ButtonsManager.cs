using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour 
{
    public void PlayGameButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void PassButton()
    {
        if(GameManager.Instance.CurrentPlayer == false)
        {
            Debug.Log("El jugador 1 se rindio");
            ContextGame.contextGame.playerFairies.GetComponent<Player>().PlayerPassed = true;
        }
        else if(GameManager.Instance.CurrentPlayer == true)
        {
            Debug.Log("El jugador 2 se rindio");
            ContextGame.contextGame.playerDemons.GetComponent<Player>().PlayerPassed = true;
        }
        GameManager.Instance.ChangePlayerTurn();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}