using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FavoriteWindow : Window
{

    [SerializeField] private UserCardsScrollView _usersList;

    private void OnEnable()
    {
        CreateList();
    }

    private void OnDisable()
    {
        _usersList.ClearPool();
    }

    private void CreateList()
    {
        UserData[] allUsers = DataManager.Instance.UsersData;
        UserData[] selection = allUsers.ToList().FindAll(u => DataManager.Instance.Favorites.ContainsUser(u.Id)).ToArray();

        _usersList.Init(selection);
    }
   
}
