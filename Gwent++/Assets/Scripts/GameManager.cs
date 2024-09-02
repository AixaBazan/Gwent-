using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameStart, RoundStart, Turn, RoundEnd, GameEnd
}

public class GameManager : MonoBehaviour
{
    public bool CurrentPlayer{get; private set;} //si es false el turno es d fairies, si es true el turno es d demons
    public void ChangePlayerTurn()
    {
        //revisar para cuando se pase
        this.CurrentPlayer = !CurrentPlayer;
    }
    public static GameManager Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Debug.Log(CardDataBase.CreatedCards.Count);
        //hacer metodo de llenar los decks
        //Se asigna el primer jugdor
        CurrentPlayer = false;
      
        // Agregar las cartas a la lista
        ContextGame.contextGame.playerFairies.GetComponent<Player>().Deck.AddRange(CardDataBase.CreatedCards);

        // Comprobar los prefabs cargados
        foreach (Card card in ContextGame.contextGame.playerFairies.GetComponent<Player>().Deck)
        {
            Debug.Log("Carta cargada: " + card.name);
            if(card.effects == null)
            {
               Debug.Log("efectos no encontrados");
            }
            else
            {
                Debug.Log(card.effects.Count);
                foreach(var item in card.effects)
                {
                    Debug.Log(item);
                }
            }  
        }
        ContextGame.contextGame.Shuffle(ContextGame.contextGame.playerFairies.GetComponent<Player>().Deck);
        ContextGame.contextGame.Stole(ContextGame.contextGame.playerFairies.GetComponent<Player>());
        ContextGame.contextGame.Stole(ContextGame.contextGame.playerFairies.GetComponent<Player>());
        ContextGame.contextGame.Stole(ContextGame.contextGame.playerFairies.GetComponent<Player>());
        ContextGame.contextGame.UpdateFront();

        // GameObject[] loadedPrefabs2 = Resources.LoadAll<GameObject>("DemonsCard");
        // player2.Deck.AddRange(loadedPrefabs2);
        // // Comprobar los prefabs cargados
        // foreach (GameObject prefab in player2.Deck)
        // {
        //     Debug.Log("Prefab cargado: " + prefab.name);
        // }
        // ContextGame.Shuffle(player2.Deck);
    }
    void Update()
    {
        
    }
}
