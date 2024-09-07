using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class Zone : MonoBehaviour
{
    public List<Card> Cards;
    public double UpdateZone()
    {
        double counter = 0;

        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
   
        GameObject[] children = GetChildrenFromGridLayout(gridLayoutGroup);

        // Ejemplo: Imprimir los nombres de los hijos y  destruirlos
        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        //Instanciar los nuevos gameObjects segun las cartas q estan en la lista
        foreach (Card card in Cards)
        {
            if(CompareTag("Hand"))
            {
                card.IsPlayed = false;
            }
            counter += card.Power;
            InstantiateCard(card, gridLayoutGroup);
        }
        return counter;
    }
    //toma los gameObjects q estan actualmente en la zona dada
    private GameObject[] GetChildrenFromGridLayout(GridLayoutGroup grid)
    {
        // Obtén el número de hijos
        int childCount = grid.transform.childCount;

        // Crea un array para almacenar los hijos
        GameObject[] children = new GameObject[childCount];

        // Llena el array con los hijos
        for (int i = 0; i < childCount; i++)
        {
            children[i] = grid.transform.GetChild(i).gameObject;
        }

        return children;
    }

    public void InstantiateCard(Card card, GridLayoutGroup gridLayoutGroup)
    {
        // Instanciar el prefab y asignar el Scriptable Object
        GameObject cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/CardPrefab.prefab");
        GameObject cardCopy = GameObject.Instantiate(cardPrefab);
        // Asigna el Scriptable Object al componente CardDisplay
        cardCopy.GetComponent<CardDisplay>().card = (Card)card;
        string name = cardCopy.GetComponent<CardDisplay>().card.Name;

        // Establecer el padre del nuevo GameObject al GridLayoutGroup
        cardCopy.transform.SetParent(gridLayoutGroup.transform, false);
    }
}
