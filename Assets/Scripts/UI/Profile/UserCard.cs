using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserCard : PoolScrollItem<UserData>
{
    [field: SerializeField] public Image _userIcon, _backgroundImage;
    [field: SerializeField] public TextMeshProUGUI _usernameText, _title1Text, _title2Text;
    [field: SerializeField] public Toggle _favoriteToggle;

    [SerializeField] private Sprite _defaultIcon;
    public UserData UserData { get; private set; }

    public void Init(UserData uData)
    {
        UserData = uData;
        _usernameText.text = $"{uData.FirstName} {uData.LastName}";
        _title1Text.text = uData.Email;
        _title2Text.text = uData.IpAddress;

        string filename = DataManager.Instance.AvatarsData.Data.ToList().Find(u => u.Id == uData.Id).FileName;
        if ( DataManager.Instance.TexturesCaches.ToList().Exists(c => c.Name == filename))
        {
            var texCache = DataManager.Instance.TexturesCaches.ToList().Find(c => c.Name == filename);
            _userIcon.sprite =texCache.Texture;
        }
        else
        {
            _userIcon.sprite = _defaultIcon;
        }

        if(DataManager.Instance.Favorites.ContainsUser(uData.Id))
        {
            _favoriteToggle.isOn = true;
        }
        else
            _favoriteToggle.isOn= false;
    }

    public override void SetInfo(UserData info)
    {
        Init(info);
    }
    public void SetInFavorites(bool val)
    {

        if (val)
            DataManager.Instance.Favorites.AddUser(UserData.Id);
        else
            DataManager.Instance.Favorites.RemoveUser(UserData.Id);

        DataManager.Instance.SaveFavorites();
    }

    public void OpenProfile()
    {
        WindowsManager.Instance.CloseWindows();
        ProfileWindow pWindow = (ProfileWindow)WindowsManager.Instance.OpenWindow(WindowsManager.Instance.ProfileWindow);
        pWindow.Init(UserData);
    }

}
