using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;


public class GoogleAds : MonoBehaviour
{
    private BannerView _bannerView;
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;
    private const string ADUnitReward = "ca-app-pub-3940256099942544/5224354917";
    private const string ADUnitBanner = "ca-app-pub-3940256099942544/6300978111";
    private const string ADUnitInterstitial = "ca-app-pub-3940256099942544/1033173712";
    private float _timer = 100f;


    // Start is called before the first frame update
    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        this._rewardedAd = new RewardedAd(ADUnitReward);
        RequestBanner();
        RequestReward();
        RequestInterstitial();
    }

    public void ShowReward()
    {
        StartCoroutine(ShowRewardedIE());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator ShowRewardedIE()
    {
        while (!_rewardedAd.IsLoaded())
            yield return null;

        _rewardedAd.Show();
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    private static void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        GameManager.Instance.RewardLives();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator ShowInterstitialIE()
    {
        while (!_interstitialAd.IsLoaded())
            yield return null;
        _interstitialAd.Show();
    }

    private void ShowInterAd()
    {
        if (_timer <= 0)
        {
            StartCoroutine(ShowInterstitialIE());
            _timer = 100.0f; //Amount of seconds
        }
    }


    private void RequestBanner()
    {
        _bannerView = new BannerView(ADUnitBanner, AdSize.Banner, AdPosition.Top);
        var request = new AdRequest.Builder().Build();
        this._bannerView.LoadAd(request);
    }


    private void RequestReward()
    {
        _rewardedAd = new RewardedAd(ADUnitReward);
        var request = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(request);
    }

    private void RequestInterstitial()
    {
        _interstitialAd = new InterstitialAd(ADUnitInterstitial);
        var request = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(request);
    }

    // Update is called once per frame
    private void Update()
    {
        _timer -= Time.deltaTime;
        ShowInterAd();
    }
}