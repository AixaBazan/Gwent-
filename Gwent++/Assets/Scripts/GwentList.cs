using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
class GwentList : IList<GameObject>
{
    //Falta Find y  Remove
    public List<GameObject> List{get; private set;}
    public GwentList()
    {
        this.List = new List<GameObject>();
    }
    static System.Random random = new System.Random();
    public void Shuffle()
    {
        for (int i = 0; i < List.Count; i++)
        {
            int index = random.Next(0, List.Count - 1);
            //Change
            GameObject Temp = List[i];
            List[i] = List[index];
            List[index] = Temp;
        }
    }
    public void Push(GameObject item) => List.Add(item);
    public void SendBottom(GameObject item) => List.Insert(0, item);
    public GameObject Pop()
    {
        GameObject card = List[List.Count -1];
        List.Remove(card);
        return card;
    }
    public void Insert(int index, GameObject item)
    {
        List.Insert(index, item);
    }
    public void RemoveAt (int index)
    {
        List.RemoveAt(index);
    }
    public int IndexOf(GameObject item)
    {
        return List.IndexOf(item);
    }
    public GameObject this[int index]{get{return List[index];} set{List[index] = value;}}
    public void Add(GameObject item)
    {
        List.Add(item);
    }
    //revisar si mandarlo al cementerio directamente o hacerlo en el EventTrigger: hacer Remove
    public bool Remove(GameObject item)
    {
        return List.Remove(item);
    }
    public bool Contains(GameObject item)
    {
        return List.Contains(item);
    }
    public int Count {get{return List.Count;}}
    public void Clear()
    {
        List.Clear();
    }
    public bool IsReadOnly{get{return false;}}
    public void CopyTo(GameObject[] array, int arrayIndex)
    {
        List.CopyTo(array, arrayIndex);
    }
    public IEnumerator<GameObject> GetEnumerator()
    {
        return List.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}