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
    public void MoveCard(GameObject card)
    {
        Card cardProperty = card.GetComponent<CardDisplay>().card;
        GameObject Zone;
        Player owner = ContextGame.contextGame.GetPlayer(cardProperty.Owner);

        if(cardProperty.GameZone.Contains("Melee"))
        {
           Zone = owner.MeleeZone;
           Move(Zone, card);
        }
        else if(cardProperty.GameZone.Contains("Ranged"))
        {
            Zone = owner.RangedZone;
            Move(Zone, card);
        }
        else if(cardProperty.GameZone.Contains("Siege"))
        {
            Zone = owner.SiegeZone;
            Move(Zone, card);
        }
    }
    private void Move(GameObject Zone, GameObject Target)
    {
        Target.transform.SetParent(Zone.transform, false);
        GameManager.Instance.ChangePlayerTurn();
    }
}
