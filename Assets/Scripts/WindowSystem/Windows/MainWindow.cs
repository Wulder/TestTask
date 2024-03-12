using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindow : Window
{
    [SerializeField] private UserCardsScrollView _usersList;

    private bool _isInit = false;
    private void OnEnable()
    {
        if(!_isInit)
        {
            CreateList(DataManager.Instance.UsersData);
            _isInit = true;
        }
        _usersList.UpdateItemsInPool();
    }


    private void CreateList(UserData[] users)
    {
        _usersList.Init(users);
    }
}
