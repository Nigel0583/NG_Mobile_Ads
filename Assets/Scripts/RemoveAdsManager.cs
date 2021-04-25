using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsManager : Singleton<RemoveAdsManager>
{
    public enum ItemType
    {
        NoAds
    }

    public ItemType item;
    private string defaultText;
    public Text TextPrice;
    
    private void Start()
    {
        StartCoroutine(LoadPriceRout());
        defaultText = TextPrice.text;
    }

    public void ClickBuy()
    {
        switch (item)
        {
            case ItemType.NoAds:
                IAPManager.Instance.BuyNoAds();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator LoadPriceRout()
    {
        while (!IAPManager.IsInitialized())
            yield return null;

        var loadPrice = item switch
        {
            ItemType.NoAds => IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.NoAds),
            _ => throw new ArgumentOutOfRangeException()
        };

        TextPrice.text = defaultText + " " + loadPrice;
    }
}