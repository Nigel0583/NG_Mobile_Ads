using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : Singleton<PurchaseManager>
{
    public enum ItemType
    {
        NoAds,
        ColorMod
    }

    public ItemType item;
    private string _defaultText;
    public Text TextPrice;
    private string _defaultText1;
    public Text TextPrice1;

    private void Start()
    {
        StartCoroutine(LoadPriceRout());
        _defaultText = TextPrice.text;
        _defaultText1 = TextPrice1.text;
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
        var loadPrice1 = "";
        switch (item)
        {
            case ItemType.NoAds:
                loadPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.NoAds);
                break;
            case ItemType.ColorMod:
                loadPrice1 = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.ColorMod);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        TextPrice.text = _defaultText + " " + loadPrice;
        TextPrice1.text = _defaultText1 + " " + loadPrice1;
    }
}