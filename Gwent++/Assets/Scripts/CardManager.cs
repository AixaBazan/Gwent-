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
        GameObject Zone;
       
        Player owner = ContextGame.contextGame.GetPlayer(card.Owner);

        GameObject Hand = owner.HandZone;

        if(card.GameZone.Contains("Melee"))
        {
           Zone = owner.MeleeZone;
           Move(Zone, owner, card);
        }
        else if(card.GameZone.Contains("Ranged"))
        {
            Zone = owner.RangedZone;
            Move(Zone, owner, card);
        }
        else if(card.GameZone.Contains("Siege"))
        {
            Zone = owner.SiegeZone;
            Move(Zone, owner, card);
        }
    }
    private void Move(GameObject Zone, Player player, Card Target)
    {
        Zone.GetComponent<Zone>().Cards.Add(Target);
        player.Field.Add(Target);
        player.HandZone.GetComponent<Zone>().Cards.Remove(Target);
        Target.ExecuteEffect();
        ContextGame.contextGame.UpdateFront();
    }
}
