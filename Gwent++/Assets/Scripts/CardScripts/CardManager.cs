using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
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
    public void MoveCard(Card card)
    {
        GameObject Zone = null;
       
        Player owner = ContextGame.contextGame.GetPlayer(card.Owner);

        if(card.Type == CardType.Lider)
        {
            card.IsPlayed = true;
            card.ExecuteEffect();
            ContextGame.contextGame.UpdateFront();
        }
        else if(card.Type == CardType.Señuelo)
        {
            card.ExecuteEffect();
            ContextGame.contextGame.UpdateFront();
        }
        else
        {
            card.IsPlayed = true;
            if(card.GameZone.Contains(ValidZone.Melee))
            {
            Zone = owner.MeleeZone;
            }
            else if(card.GameZone.Contains(ValidZone.Ranged))
            {
                Zone = owner.RangedZone;
            }
            else if(card.GameZone.Contains(ValidZone.Siege))
            {
                Zone = owner.SiegeZone;
            }
            else if(card.GameZone.Contains(ValidZone.IncreaseMelee))
            {
                Zone = owner.IncreaseMelee; 
            }
            else if(card.GameZone.Contains(ValidZone.IncreaseRanged))
            {
                Zone = owner.IncreaseRanged;
            }
            else if(card.GameZone.Contains(ValidZone.IncreaseSiege))
            {
                Zone = owner.IncreaseSiege;
            }
            else if(card.GameZone.Contains(ValidZone.WeatherZone))
            {
                Zone = ContextGame.contextGame.WeatherZone;
            }
            Move(Zone, owner, card);
        }
    }
    private void Move(GameObject zone, Player player, Card card)
    {
        zone.GetComponent<Zone>().Cards.Add(card);
        player.HandZone.GetComponent<Zone>().Cards.Remove(card);
        card.ExecuteEffect();
        ContextGame.contextGame.UpdateFront();
    }
}