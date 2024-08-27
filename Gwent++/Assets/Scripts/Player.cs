using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Player : MonoBehaviour
{
    void Start()
    {
        this.Cementery = new List<GameObject>();
        this.Deck = new List<GameObject>();
    }
    public int ID;
    public GameObject HandZone; 
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    public List<GameObject> Deck;
    public List<GameObject> Cementery;
    public List<GameObject> Field ()
    {
        List<GameObject> field = new List<GameObject>();
        field = new List<GameObject>();
        field.AddRange(MeleeZone.GetComponent<Zone>().Cards);
        field.AddRange(RangedZone.GetComponent<Zone>().Cards);
        field.AddRange(SiegeZone.GetComponent<Zone>().Cards);
        return field;
    }
}