using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUsersCatalog : MonoBehaviour
{
    [SerializeField] private UserCard _cardPrefab;
    [SerializeField] private GameObject _contentRoot;

    private List<UserCard> _cards = new List<UserCard>();

    private UserData[] _data;


    public void Clear()
    {
        for(int i = 0; i < _cards.Count; i++) 
        {
            Destroy(_cards[i].gameObject);
        }

        _cards.Clear();
    }



    public void CreateList(UserData[] data)
    {
        foreach (var user in data)
        {
            var uc = Instantiate(_cardPrefab, _contentRoot.transform);
            uc.Init(user);
            _cards.Add(uc);
        }
      
    }

    public void UpdateList(UserData[] data)
    {
        Clear();
        CreateList(data);
    }
}
