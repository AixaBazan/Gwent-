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
            GameManager.Instance.cartelManager.GetComponent<CartelManager>().MostrarCartel("El jugador Fairies se rindio");
            ContextGame.contextGame.playerFairies.GetComponent<Player>().PlayerPassed = true;
        }
        else if(GameManager.Instance.CurrentPlayer == true)
        {
            GameManager.Instance.cartelManager.GetComponent<CartelManager>().MostrarCartel("El jugador Demons se rindio");
            ContextGame.contextGame.playerDemons.GetComponent<Player>().PlayerPassed = true;
        }
        GameManager.Instance.ChangePlayerTurn();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void CardBuilder()
    {
        SceneManager.LoadScene("CardBuilder");
    }
}