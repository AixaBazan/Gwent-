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
        this.Increase = new GameObject[3]{IncreaseMelee, IncreaseRanged, IncreaseSiege};
        this.Field = new GameObject[3]{MeleeZone, RangedZone, SiegeZone};
        PlayerPassed = false;
        RoundsWon = 0;
        Points = 0;
        Rounds.text = RoundsWon.ToString();
    }
    public bool PlayerPassed;
    public CardFaction Faction;
    public int ID;
    public int RoundsWon;
    public TMP_Text Rounds;
    public double Points;
    public TMP_Text Counter;
    public GameObject[] Field {get; private set;}
    public GameObject HandZone; 
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    public List<Card> Deck;
    public List<Card> Cementery;
    public GameObject[] Increase {get; private set;}
    public GameObject IncreaseMelee;
    public GameObject IncreaseRanged;
    public GameObject IncreaseSiege;
    public List<Card> GetField()
    {
        List<Card> field = new List<Card>();
        for(int i = 0; i < 3; i++)
        {
            field.AddRange(Field[i].GetComponent<Zone>().Cards);
        }
        return field;
    }
    public GameObject LeaderZone;
    public Card Leader;
    public int UpdateRounds()
    {
        Rounds.text = RoundsWon.ToString();
        return RoundsWon;
    }
    public void UpdateCounter(double value)
    {
        this.Points = value;
        Counter.text = Points.ToString();
    }
}