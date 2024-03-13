using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class WindowsManager : Singleton<WindowsManager>
{

    public Action<Window> OnOpenWindow, OnCloseWindow;

    private Window _currentWindow;

    [field: SerializeField] public MainWindow MainWindow {  get; set; }
    [field: SerializeField] public FavoriteWindow FavoriteWindow { get; set; }
    [field: SerializeField] public ProfileWindow ProfileWindow { get; set; }

    [SerializeField] private List<WindowPuttonPair> _windowButtonsPairs = new List<WindowPuttonPair>();


    public Window OpenWindow(Window window)
    {
        window.Open();
        _currentWindow = window;
        OnOpenWindow?.Invoke(window);

        

        if (_windowButtonsPairs.Exists(p => p.Window == window))
        {
            var pair = _windowButtonsPairs.Find(p => p.Window == window);

            foreach (var p in _windowButtonsPairs)
                p.Button.sprite = null;

            pair.Button.sprite = pair.ActiveSprite;
        }
        return _currentWindow;
    }
    public void CloseWindows()
    {
        if(_currentWindow)
        {
            _currentWindow.Close();
            OnCloseWindow?.Invoke(_currentWindow);
            _currentWindow = null;
        }
    }
    public void GoToWIndow(Window window)
    {
        CloseWindows();
        OpenWindow(window);
        
    }

   
}

[Serializable]
public struct WindowPuttonPair
{
    [field: SerializeField] public Window Window { get; set; }
    [field: SerializeField] public Image Button { get; set; }
    [field: SerializeField] public Sprite ActiveSprite { get; set; }
}
