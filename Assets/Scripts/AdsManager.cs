using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : Singleton<AdsManager>, IUnityAdsListener
{
    private const string GameId = "4102803";
    private bool _testMode = true;
    private const string PlacementName = "rewardedVideo";
    private const string BannerName = "bannerAd";

    // Start is called before the first frame update
    private void Start()
    {
        Advertisement.Initialize(GameId);
        Advertisement.AddListener(this);
        StartCoroutine(nameof(DisplayBannerReady));
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public override void Awake()
    {
    }

    private IEnumerator DisplayBannerReady()
    {
        while (!Advertisement.IsReady(BannerName))
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(BannerName);
    }

    public static void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void DisplayInterstitial()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    public void DisplayRewardAd()
    {
        if (!Advertisement.IsReady(PlacementName)) return;
        HideBanner();
        Advertisement.Show(PlacementName);
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            GameManager.Instance.RewardLives();
        }
    }
}