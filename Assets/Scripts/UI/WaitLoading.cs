using UnityEngine;

public class WaitLoading : MonoBehaviour
{

    

    [SerializeField] private GameObject _loadingPanel;
    void Awake()
    {
        InvokeRepeating(nameof(CheckStatus), 1, 1);
    }

    void CheckStatus()
    {
        var data = DataManager.Instance;
        if(data.UsersData != null && data.Favorites != null && data.TexturesCaches != null)
        {
            _loadingPanel.SetActive(false);
            CancelInvoke(); //колхоз, но я устал
            WindowsManager.Instance.GoToWIndow(WindowsManager.Instance.MainWindow);
        }
    }
}
