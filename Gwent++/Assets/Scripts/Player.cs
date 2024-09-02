using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Player : MonoBehaviour
{
    void Start()
    {
        this.Cementery = new List<Card>();
        this.Deck = new List<Card>();
    }
    public int ID;
    public GameObject HandZone; 
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    public List<Card> Deck;
    public List<Card> Cementery;
    public List<Card> Field ()
    {
        List<Card> field = new List<Card>();
        field.AddRange(MeleeZone.GetComponent<Zone>().Cards);
        field.AddRange(RangedZone.GetComponent<Zone>().Cards);
        field.AddRange(SiegeZone.GetComponent<Zone>().Cards);
        return field;
    }
}