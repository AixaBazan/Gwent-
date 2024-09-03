using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
public class Player : MonoBehaviour
{
    void Start()
    {
        this.Cementery = new List<Card>();
    }
    public string Faction;
    public int ID;
    public int WinnedRounds = 0;
    public TMP_Text Rounds;
    public double Points = 0;
    public TMP_Text Counter;
    public GameObject HandZone; 
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    public List<Card> Deck;
    public List<Card> Cementery;
    public List<Card> Field;
    public int UpdateRounds()
    {
        Rounds.text = WinnedRounds.ToString();
        return WinnedRounds;
    }
    public void UpdateCounter(double value)
    {
        Counter.text = value.ToString();
    }
}