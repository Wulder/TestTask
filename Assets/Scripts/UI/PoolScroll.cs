using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolScroll<T, I> : MonoBehaviour where I : PoolScrollItem<T>
{
    [SerializeField] protected GameObject _contentRoot;
    [SerializeField] protected I _itemPrefab;
    [SerializeField] protected int _poolSize;
   protected int _validPoolSize { get { if (ItemsData != null && ItemsData.Length < _poolSize) return ItemsData.Length; return _poolSize; }}

    
    private I[] _items;
    private T[] _itemsData;

    private int _currentPointer = 0;

    public int Pointer => _currentPointer;
    public I[] Items => _items;
    public T[] ItemsData => _itemsData;

    public void Init(T[] data)
    {
     


        _itemsData = data;
        _items = new I[_validPoolSize];
        CreatePoolObjects();
        OnInit();
    }
    private void CreatePoolObjects()
    {
        for (int i = 0; i < _validPoolSize; i++)
        {
            _items[i] = CreateItem(i);
            _items[i].SetInfo(_itemsData[i]);
        }
    }
    public void ClearPool()
    {
        if(_items == null) return;
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null)
            {
                Destroy(_items[i].gameObject);
            }

           
        }
        _items = null;
    }
    protected virtual I CreateItem(int id)
    {
        return Instantiate(_itemPrefab, _contentRoot.transform);
    }

    public virtual void UpdateItemsInPool()
    {
        for (int i = 0; i < _validPoolSize; i++)
        {
            _items[i].SetInfo(ItemsData[_currentPointer + i]);
        }
    }

    protected virtual void OnInit() { }
    public void ScrollPreview()
    {
        if (_currentPointer <= 0) return;
        _currentPointer--;
        UpdateItemsData();

        OnScrollPreview();
        OnScroll();
    }

    public void ScrollNext()
    {
        if (_currentPointer >= _itemsData.Length - _validPoolSize) return;
        _currentPointer++;
       


        OnScrollNext();
        OnScroll();
    }

    private void UpdateItemsData()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetInfo(_itemsData[_currentPointer + i]);
        }

    }
    protected virtual void OnScrollPreview()
    {
        UpdateItemsData();
    }

    protected virtual void OnScrollNext()
    {
        OnScrollPreview();
    }

    protected virtual void OnScroll()
    {

    }

    #region Control

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) ScrollNext();
        if (Input.GetKeyDown(KeyCode.UpArrow)) ScrollPreview();
    }
#endif

    #endregion


}
