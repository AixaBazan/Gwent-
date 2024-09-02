using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Player : MonoBehaviour
{
    void Start()
    {
        this.Cementery = new List<Card>();
    }
    public string Faction;
    public int ID;
    public int WinnedRounds = 0;
    public int Points = 0;
    public GameObject HandZone; 
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    public List<Card> Deck;
    public List<Card> Cementery;
    public List<Card> Field;
}