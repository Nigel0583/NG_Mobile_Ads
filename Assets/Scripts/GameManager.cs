using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject resetButton;
    public GameObject getLivesButton;
    public GameObject rewardButton;
    public GameObject leaderButton;
    public GameObject achieveButton;
    public Text txtLives;
    public float timeRemaining = 15;
    public bool timerIsRunning = false;
    public Text timeText;
    public GameObject vicText;
    public GameObject noLivesText;
    private bool _noAds = false;
    public GameObject noAdsButton;

    private static int CntLive { get; set; }
    private float _score = 0;


    private void Awake()
    {
        Instance = this;
    }

    public void RemoveAds()
    {
        _noAds = true;
        noAdsButton.SetActive(false);
    }

    public void Reset()
    {
        PlayGamesClient.AddScoreToLeaderboard(GPGSIds.leaderboard_lives, CntLive);
        CntLive = 0;
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    private void Start()
    {
        CntLive = 2;
        timerIsRunning = true;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Update is called once per frame
    private void Update()
    {
        if (!timerIsRunning) return;
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            RemoveLives();
        }
    }


    public void ScoreKeep()
    {
        _score++;
        if (_score >= 4)
        {
            Victory();
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private async Task Victory()
    {
        timerIsRunning = false;
        timeRemaining = 15;
        await Task.Delay(1000);
        UnlockClearAll();
        vicText.SetActive(true);
        leaderButton.SetActive(true);
        achieveButton.SetActive(true);
        resetButton.SetActive(true);
        rewardButton.SetActive(true);
        noAdsButton.SetActive(true);
    }

    public void RewardLives()
    {
        timerIsRunning = true;
        timeRemaining = 30;
        CntLive++;
        UnlockMoreLives();
        txtLives.text = "Lives: " + CntLive;
        noLivesText.SetActive(false);
        getLivesButton.SetActive(false);
    }

    private async Task RemoveLives()
    {
        timerIsRunning = true;
        timeRemaining = 15;
        CntLive--;
        if (CntLive == 0)
        {
            timerIsRunning = false;
            await Task.Delay(1000);
            noLivesText.SetActive(true);
            getLivesButton.SetActive(true);
        }
        else
        {
            txtLives.text = "Lives: " + CntLive;
            timerIsRunning = true;
            timeRemaining = 15;
        }
    }

    private static void UnlockMoreLives()
    {
        PlayGamesClient.UnlockAchievement(GPGSIds.achievement_get_more_lives);
    }

    private static void UnlockClearAll()
    {
        PlayGamesClient.UnlockAchievement(GPGSIds.achievement_clear_all);
    }
    public void ShowAchievements()
    {
        PlayGamesClient.ShowAchievementsUI();
    }
     
    public void ShowLeaderboards()
    {
        PlayGamesClient.ShowLeaderboardsUI();
    }
}