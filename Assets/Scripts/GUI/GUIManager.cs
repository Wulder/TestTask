using UnityEngine;


public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] private GameObject _bottomPanel, _upperPanel;

    
    public void HideBottomPanel()
    {
        _bottomPanel.SetActive(false);
    }

    public void ShowBottomPanel()
    {
        _bottomPanel.SetActive(true);
    }

}
