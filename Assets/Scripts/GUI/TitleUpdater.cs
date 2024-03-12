using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUpdater : MonoBehaviour
{
    [SerializeField] private WindowsManager _windowsManager;
    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private string _prefix, _postfix;
    private void OnEnable()
    {
        _windowsManager.OnOpenWindow += UpdateTitle;
    }

    private void OnDisable()
    {
        _windowsManager.OnOpenWindow -= UpdateTitle;
    }

    public void UpdateTitle(Window window)
    {
        _titleText.text = $"{_prefix}{window.WindowName}{_postfix}";
    }
}
