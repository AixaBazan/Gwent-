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
        PlayerPassed = false;
        RoundsWon = 0;
        Points = 0;
        Rounds.text = RoundsWon.ToString();
    }
    public bool PlayerPassed;
    public string Faction;
    public int ID;
    public int RoundsWon;
    public TMP_Text Rounds;
    public double Points;
    public TMP_Text Counter;
    public GameObject HandZone; 
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    public List<Card> Deck;
    public List<Card> Cementery;
    public GameObject[] Increase;
    public GameObject IncreaseMelee;
    public GameObject IncreaseRanged;
    public GameObject IncreaseSiege;
    //revisar field
    public List<Card> Field()
    {
        List<Card> field = new List<Card>();
        field.AddRange(MeleeZone.GetComponent<Zone>().Cards);
        field.AddRange(RangedZone.GetComponent<Zone>().Cards);
        field.AddRange(SiegeZone.GetComponent<Zone>().Cards);
        return field;
    }
    public int UpdateRounds()
    {
        Rounds.text = RoundsWon.ToString();
        return RoundsWon;
    }
    public void UpdateCounter(double value)
    {
        Points = value;
        Counter.text = value.ToString();
    }
}