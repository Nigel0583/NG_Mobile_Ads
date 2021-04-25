using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    private const string GameId = "4102803";
    private bool _testMode = true;
    private static AdsManager _adsInstance;
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

    private void Awake()
    {
        _adsInstance = this;
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

    public void DisplayAds()
    {
        if (!Advertisement.IsReady()) return;
        Advertisement.Show();
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