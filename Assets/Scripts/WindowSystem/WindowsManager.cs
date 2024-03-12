using System;
using UnityEngine;

public class WindowsManager : Singleton<WindowsManager>
{

    public Action<Window> OnOpenWindow, OnCloseWindow;

    private Window _currentWindow;

    [field: SerializeField] public MainWindow MainWindow {  get; set; }
    [field: SerializeField] public FavoriteWindow FavoriteWindow { get; set; }
    [field: SerializeField] public ProfileWindow ProfileWindow { get; set; }

    public Window OpenWindow(Window window)
    {
        window.Open();
        _currentWindow = window;
        OnOpenWindow?.Invoke(window);
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
