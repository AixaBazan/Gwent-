using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public Card card;
    public bool IsPlayed = false;
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
        CardManager.Instance.MoveCard(gameObject);
    }
}
