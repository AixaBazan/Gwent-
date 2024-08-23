using System.Collections;
using System;
using System.Collections.Generic;
class GwentList : IList<int>
{
    //Falta Find y  Remove
    public List<int> List{get; private set;}
    public GwentList()
    {
        this.List = new List<int>();
    }
    static Random random = new Random();
    public void Shuffle()
    {
        for (int i = 0; i < List.Count; i++)
        {
            int index = random.Next(0, List.Count - 1);
            //Change
            int Temp = List[i];
            List[i] = List[index];
            List[index] = Temp;
        }
    }
    public void Push(int item) => List.Add(item);
    public void SendBottom(int item) => List.Insert(0, item);
    public int Pop()
    {
        int card = List[List.Count -1];
        List.Remove(card);
        return card;
    }
    public void Insert(int index, int item)
    {
        List.Insert(index, item);
    }
    public void RemoveAt (int index)
    {
        List.RemoveAt(index);
    }
    public int IndexOf(int item)
    {
        return List.IndexOf(item);
    }
    public int this[int index]{get{return List[index];} set{List[index] = value;}}
    public void Add(int item)
    {
        List.Add(item);
    }
    //revisar si mandarlo al cementerio directamente o hacerlo en el EventTrigger: hacer Remove
    public bool Remove(int item)
    {
        return List.Remove(item);
    }
    public bool Contains(int item)
    {
        return List.Contains(item);
    }
    public int Count {get{return List.Count;}}
    public void Clear()
    {
        List.Clear();
    }
    public bool IsReadOnly{get{return false;}}
    public void CopyTo(int[] array, int arrayIndex)
    {
        List.CopyTo(array, arrayIndex);
    }
    public IEnumerator<int> GetEnumerator()
    {
        return List.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}