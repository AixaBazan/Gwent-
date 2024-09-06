using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public Card card;
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
    void Update()
    {
        power.text = card.Power.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ContextGame.contextGame.GetPlayer(card.Owner).CanChange = false;
            if(card.Owner == 1 && GameManager.Instance.CurrentPlayer == false && card.IsPlayed == false)
            {
                CardManager.Instance.MoveCard(card);
                GameManager.Instance.ChangePlayerTurn();
            }
            else if(card.Owner == 2 && GameManager.Instance.CurrentPlayer == true && card.IsPlayed == false)
            {
                CardManager.Instance.MoveCard(card);
                GameManager.Instance.ChangePlayerTurn();
            }
            else
            {
                GameManager.Instance.cartelManager.GetComponent<CartelManager>().MostrarCartel("No es su turno o la carta ya esta en el campo");
            }
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            ContextGame.contextGame.ChangeCard(ContextGame.contextGame.GetPlayer(card.Owner), card);
        }
    }
}
