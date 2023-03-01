using UnityEngine;
using TMPro;

[System.Serializable]
public enum RewardType
{
    MARKET,
    HOMEITEM,
    WIN,
    OVER
}

[System.Serializable]
public struct Juri
{
    public Animator m_Anim;
    public GameObject Canvas;
    public TextMeshProUGUI ScoreText;

}

public class LevelManager : MonoBehaviour
{
    private const string CurrentLevel = "CurrentLevel";
    private const string Level = "Level";

    #region Value Settings
    [Header("Panels")]
    [SerializeField] private GameObject WinPanel, OverPanel;
    public TextMeshProUGUI WinScoreText;
    [Header("Level Text")]
    [SerializeField] private TextMeshProUGUI currentLevelSize; // Run Level Size
    public GameObject OnEarnWinButton;
    PanelController panelController;
    private int _level;

    public RewardType type;
    public bool isOkAtReward = false;
    #endregion

    private void Start()
    {
        panelController = GetComponent<PanelController>();
        _level = PlayerPrefs.GetInt(Level);
        currentLevelSize.text = "Level " + _level.ToString();
        type = RewardType.HOMEITEM;
    }


    #region Admob Controller

    public void StartPanel()
    {
        BannerController(false);
    }

    public bool ShowReward()
    {
        if (AdmobManager.Instance.rewardedAd.IsLoaded())
        {
            AdmobManager.Instance.rewardedAd.Show();
            return true;
        }
        else
            return false;
    }

    public void OnShowReward()
    {
        ShowReward();
    }

    public void WinScoreAtRewardController()
    {
        OnEarnWinButton.gameObject.SetActive(false);
        int x2Score = GameManager.Current.GetScore() * 2;
        panelController.SetUserMoneyController(x2Score);
    }

    public bool isEarnAtReward(bool isOk)
    {
        if (isOk)
        {
            isOkAtReward = true;
            return true;
        }
        else
        {
            isOkAtReward = false;
            return false;
        }
    }

    public void BannerController(bool isShow)
    {
        if (isShow)
            AdmobManager.Instance.bannerView.Show();
        else
            AdmobManager.Instance.bannerView.Hide();
    }

    #endregion

    #region Win Over Panel Control
    public void Win()
    {

        if (AdmobManager.Instance.rewardedAd.IsLoaded())
            OnEarnWinButton.gameObject.SetActive(true);
        else
            OnEarnWinButton.gameObject.SetActive(false);

        OnEarnWinButton.gameObject.SetActive(true);

        panelController.OpenPanel(4);
        int score = GameManager.Current.GetScore();
        WinScoreText.text = "Earn " + score.ToString() + "$";
        panelController.CoinsController();
        BannerController(true);
    }


    public void Over()
    {
        if (AdmobManager.Instance.isReadyinterstitial())
            AdmobManager.Instance.interstitial.Show();

        OverPanel.SetActive(true);
    }

    #endregion

    #region Scene Manager
    public void RestartLevel()
    {
        int CurrentLev = PlayerPrefs.GetInt(CurrentLevel);

        LevelLoader.Current.LevelController("Level " + (CurrentLev).ToString());
    }
    public void NextLevel()
    {
        int CurrentLev = PlayerPrefs.GetInt(CurrentLevel);
        PlayerPrefs.SetInt(CurrentLevel, CurrentLev + 1);
        CurrentLev = PlayerPrefs.GetInt(CurrentLevel);
        PlayerPrefs.SetInt(Level, _level + 1);
        LevelLoader.Current.LevelController("Level " + (CurrentLev).ToString());
    }

    #endregion



}
