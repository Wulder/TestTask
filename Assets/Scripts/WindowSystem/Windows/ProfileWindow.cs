using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileWindow : Window
{
    [SerializeField] private Image _icon;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _nameText, _genderText, _title1Text, _title2Text;
    [SerializeField] private Sprite _defaultIcon;

    private UserData _currentUser;
    public void BackWindow()
    {
        WindowsManager.Instance.GoToWIndow(WindowsManager.Instance.MainWindow);
    }

    public void SetInFavorites(bool val)
    {

        if (val)
            DataManager.Instance.Favorites.AddUser(_currentUser.Id);
        else
            DataManager.Instance.Favorites.RemoveUser(_currentUser.Id);

        DataManager.Instance.SaveFavorites();
    }
    public override void Open()
    {
        base.Open();
        GUIManager.Instance.HideBottomPanel();
    }

    public void Init(UserData uData)
    {
        _currentUser = uData;
        if(DataManager.Instance.Favorites.ContainsUser(uData.Id))
            _toggle.isOn = true;
        else
            _toggle.isOn = false;

        if (DataManager.Instance.TexturesCaches.ToList().Exists(c => c.Id == uData.Id))
        {
            var texCache = DataManager.Instance.TexturesCaches.ToList().Find(c => c.Id == uData.Id);
            _icon.sprite = Sprite.Create(texCache.Texture, new Rect(0, 0, 256, 256), new Vector2(0, 0));
        }
        else
        {
            _icon.sprite = _defaultIcon;
        }

        _nameText.text = $"{uData.FirstName} {uData.LastName}";
        _genderText.text = uData.Gender;
        _title1Text.text = uData.Email;
        _title2Text.text = uData.IpAddress;

    }

}
