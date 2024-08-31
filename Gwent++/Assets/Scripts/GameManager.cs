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
        Debug.Log("Empezo esto");
        //Se asigna el primer jugdor
        CurrentPlayer = false;

        //Se annaden las cartas creadas por el usuario a cada Deck
        // Cargar todos los prefabs en la carpeta Resources/Prefabs
        GameObject[] loadedPrefabs1 = Resources.LoadAll<GameObject>("FairiesCard");

        // Agregar los prefabs a la lista
        ContextGame.contextGame.playerFairies.GetComponent<Player>().Deck.AddRange(loadedPrefabs1);

        // Comprobar los prefabs cargados
        foreach (GameObject prefab in ContextGame.contextGame.playerFairies.GetComponent<Player>().Deck)
        {
            Debug.Log("Prefab cargado: " + prefab.name);
        }
        ContextGame.contextGame.Shuffle(ContextGame.contextGame.playerFairies.GetComponent<Player>().Deck);
        ContextGame.contextGame.Stole(ContextGame.contextGame.playerFairies.GetComponent<Player>());

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
