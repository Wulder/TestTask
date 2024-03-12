using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FavoritesData
{
    private HashSet<int> Ids = new HashSet<int>();

    public void AddUser(int index)
    {
        Ids.Add(index);
    }

    public void RemoveUser(int index)
    {
        Ids.Remove(index);
    }

    public bool ContainsUser(int index)
    {
        return Ids.Contains(index);
    }
}
