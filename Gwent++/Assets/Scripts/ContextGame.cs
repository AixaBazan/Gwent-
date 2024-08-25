using System;
using System.Collections.Generic;
using UnityEngine;
public class ContextGame : MonoBehaviour
{
    //array de todas las listas del juego
    Dictionary<List<GameObject>, GameObject> CardsInFront; //= new Dictionary<List<GameObject>, GameObject>[]{}; //diccionario de las listas de cartas del juego y su zona ascociada en el frontend
    public GameObject HandFairiesZone;
    public GameObject HandDemonsZone;
    public GameObject MeleeFairiesZone;
    public GameObject RangedFairiesZone;
    public GameObject SiegeFairiesZone;
    public GameObject PlayerFairies;
    public GameObject PlayerDemons;
    public Player fairies {get{return PlayerFairies.GetComponent<Player>();}private set{}}
    public Player demons {get{return PlayerDemons.GetComponent<Player>();}private set{}}
    public Player TriggerPlayer{ get; set; } // retorna el el jugador q esta jugando
    public List<Card> Board{ get; set; } //retorna todas las listas del campo, hacer metodo 
    public List<GameObject> Hand => HandOfPlayer(TriggerPlayer);
    public List<GameObject> Deck => DeckOfPlayer(TriggerPlayer);
    public List<GameObject> Field => FieldOfPlayer(TriggerPlayer);
    public List<GameObject> Graveyard => GraveyardOfPlayer(TriggerPlayer);
    public List<GameObject> HandOfPlayer(Player player)
    {
        return player.Hand;
    }
    List<GameObject> field;
    public List<GameObject> FieldOfPlayer(Player player)
    {
        return player.Field(out field);
    }
    public List<GameObject> GraveyardOfPlayer(Player player)
    {
        return player.Cementery;
    }
    public List<GameObject> DeckOfPlayer(Player player)
    {
        return player.Deck;
    }
    # region Methods
    public void Push(GameObject item, List<GameObject> list) => list.Add(item);
    public void SendBottom(GameObject item, List<GameObject> list) => list.Insert(0, item);
    public GameObject Pop(List<GameObject> list)
    {
        GameObject card = list[list.Count -1];
        list.Remove(card);
        return card;
    }
    static System.Random random = new System.Random();
    public void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int index = random.Next(0, list.Count - 1);
            //Change
            GameObject Temp = list[i];
            list[i] = list[index];
            list[index] = Temp;
        }
    }
    //Metodo q permiter robar una carta del deck
    public void Stole(Player player, GameObject Hand) 
    {
         GameObject drawCard = DeckOfPlayer(player)[0];
        // drawCard.transform.SetParent(Hand.transform, false);
        HandOfPlayer(player).Add(drawCard);
        DeckOfPlayer(player).Remove(drawCard); 
    }
    void Start()
    {
        CardsInFront = new Dictionary<List<GameObject>, GameObject>
        {
            {HandOfPlayer(fairies), HandFairiesZone}, {fairies.Melee, MeleeFairiesZone}
        };
        // Shuffle(DeckOfPlayer(PlayerFairies.GetComponent<Player>()));
        Stole(fairies, HandFairiesZone);
        UpdateFront();
    }
    public void UpdateFront()
    {
       foreach (var item in CardsInFront)
       {
            foreach(var card in item.Key)
            {
                if(!HasChild(item.Value, card))
                {
                    Debug.Log("entro aqui");
                    GameObject drawCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
                    drawCard.transform.SetParent(item.Value.transform, false);
                }
            }
       }
    }
    private bool HasChild(GameObject parent, GameObject child)
    {
        // Verificar si el hijo está en la jerarquía del padre
        return child.transform.IsChildOf(parent.transform);
    }
    #endregion
}
