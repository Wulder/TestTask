using UnityEngine;

public class Window : MonoBehaviour
{
    [field: SerializeField] public string WindowName { get; private set; }
    public virtual void Open()
    {
        GUIManager.Instance.ShowBottomPanel();
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
