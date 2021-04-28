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

        var loadPrice = "";
        switch (item)
        {
            case ItemType.NoAds:
                loadPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.NoAds);
                break;
            case ItemType.ColorMod:
                loadPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.ColorMod);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        TextPrice.text = _defaultText + " " + loadPrice;
    }
}