using System;
using System.Collections;
using UnityEngine;

public class UserCardsScrollView : PoolScroll<UserData, UserCard>
{
    [SerializeField] private RectTransform _topOffset, _bottomOffset;
    [SerializeField] private float _itemHeight;
    [SerializeField] private int _poolUpdateItemsCount = 1;


    private float _prevScrollValue = 0;
    private int _prevDirection = 0;
    private float _itemScrollStep => 1f / (float)ItemsData.Length;


    protected override UserCard CreateItem(int id)
    {
        var i = base.CreateItem(id);

        i.transform.SetSiblingIndex(_contentRoot.transform.childCount - 2);
        return i;
    }

    protected override void OnInit()
    {
        _topOffset.sizeDelta = new Vector2(_topOffset.sizeDelta.x, 0);
        _bottomOffset.sizeDelta = new Vector2(_bottomOffset.sizeDelta.x, _itemHeight * (ItemsData.Length - _validPoolSize));

        _prevScrollValue = 0;
    }

    protected override void OnScroll()
    {
        var topOffsetHeight = _itemHeight * Pointer;
        var bottomOffsetHeight = _itemHeight * (ItemsData.Length - _validPoolSize) - topOffsetHeight;

        _topOffset.sizeDelta = new Vector2(_topOffset.sizeDelta.x, topOffsetHeight);
        _bottomOffset.sizeDelta = new Vector2(_bottomOffset.sizeDelta.x, bottomOffsetHeight);
    }

    public void OnComponentScroll(Vector2 vec)
    {
        float invertedValue = (1 - vec.y);
        float scrollDela = invertedValue - _prevScrollValue;

        float step = _itemScrollStep * _poolUpdateItemsCount;

        int sign = Math.Sign(scrollDela);

       if(sign < 0 && _prevDirection > 0)
        {
            _prevScrollValue = _prevScrollValue + _itemScrollStep * _poolUpdateItemsCount;
        }

        if (Mathf.Abs(scrollDela) > step)
        {

            if (Mathf.Sign(scrollDela) > 0)
            {
                for (int i = 0; i < _poolUpdateItemsCount; i++)
                    ScrollNext();
            }
            else
            {
                for (int i = 0; i < _poolUpdateItemsCount; i++)
                    ScrollPreview();
            }

            _prevDirection = sign;
            _prevScrollValue = invertedValue;


        }


    }
}
