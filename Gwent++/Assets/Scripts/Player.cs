using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Player : MonoBehaviour 
{
    void Start()
    {
        this.Hand = new List<GameObject>();
        this.Cementery = new List<GameObject>();
        this.Deck = new List<GameObject>();
        this.Melee = new List<GameObject>();
        this.Ranged = new List<GameObject>();
        this.Siege = new List<GameObject>();

        // Cargar todos los prefabs en la carpeta Resources/Prefabs
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("FairiesCard");

        // Agregar los prefabs a la lista
        Deck.AddRange(loadedPrefabs);

        // Comprobar los prefabs cargados
        foreach (GameObject prefab in Deck)
        {
            Debug.Log("Prefab cargado: " + prefab.name);
        }
    }
    public List<GameObject> Deck;
    public List<GameObject> Melee;
    public List<GameObject> Ranged;
    public List<GameObject> Siege;
    public Player Id {get{return this;}}
    public List<GameObject> Hand;
    public List<GameObject> Cementery;
    public List<GameObject> Field (out List<GameObject> field)
    {
        field = new List<GameObject>();
        field.AddRange(Melee);
        field.AddRange(Ranged);
        field.AddRange(Siege);
        return field;
    }
}