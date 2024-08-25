using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public GameObject context;
    public Card card;
    public bool PlayedCard = false;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image image;
    public TMP_Text power;
    public TMP_Text type;
    void Start()
    {
        nameText.text = card.Name;
        descriptionText.text = card.Description;
        image.sprite = card.Image;
        power.text = card.Power.ToString();
        type.text = card.Type.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        power.text = card.Power.ToString();
    }
    //Se realiza cual se clliquea la carta
    public void OnClick()
    {
        if(card.GameZone.Contains("Melee"))
        {
            MoveCard(context.GetComponent<ContextGame>().fairies.Melee);
            //context.GetComponent<ContextGame>().UpdateFront();
        }
        // else if(card.GameZone.Contains("Siege"))
        // {
        //     MoveCard(context.GetComponent<ContextGame>().fairies.Siege);
        //     context.GetComponent<ContextGame>().UpdateFront();
        // }
        // else if(card.GameZone.Contains("Ranged"))
        // {
        //     MoveCard(context.GetComponent<ContextGame>().fairies.Ranged);
        //     context.GetComponent<ContextGame>().UpdateFront();
        // }
    }
    private void MoveCard(List<GameObject> Destiny)
    {
        Destiny.Add(this.gameObject);
        context.GetComponent<ContextGame>().HandOfPlayer(context.GetComponent<ContextGame>().fairies).Remove(this.gameObject);
        PlayedCard = true;
    }
}
