using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsManager : Singleton<RemoveAdsManager>
{
    public enum ItemType
    {
        NoAds,
        ColorMod
    }


    public ItemType item;
    private string _defaultText;
    public Text TextPrice;

    private void Start()
    {
        StartCoroutine(LoadPriceRout());
        _defaultText = TextPrice.text;

    }

    public void ClickBuy()
    {
        switch (item)
        {
            case ItemType.NoAds:
                if (PlayerPrefs.HasKey("adsRemoved") == false)
                {
                    PlayerPrefs.SetInt("adsRemoved", 0);
                    IAPManager.Instance.BuyNoAds();
                }

                break;
            case ItemType.ColorMod:
                if (PlayerPrefs.HasKey("colorMuch") == false)
                {
                    PlayerPrefs.SetInt("colorMuch", 0);
                    IAPManager.Instance.BuyColor();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator LoadPriceRout()
    {
        while (!IAPManager.IsInitialized())
            yield return null;

        ItemType loadPrice;
        switch (item)
        {
            case ItemType.NoAds:
                IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.NoAds);
                loadPrice = item;
                break;
            case ItemType.ColorMod:
                IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.ColorMod);
                loadPrice = item;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        TextPrice.text = _defaultText + " " + loadPrice;
    }
}