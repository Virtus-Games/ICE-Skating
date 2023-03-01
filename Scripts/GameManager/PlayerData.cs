using UnityEngine;

[System.Serializable]
public enum PlayerPrefEnum
{
    Player,
    Skates,
    money,
    CharackerSellMoney,
    PatentSellMoney,
    Patent0,
    Woman0,
    DressCount,
    PatentCount,
    Music,
    Vibratation,
    CurrentLevel,
}


public class PlayerData : MonoBehaviour
{
    #region Value
    private const string speed = "speed";
    private const string speedController = "speedcontrol";
    private const string levelSpeed = "speed";
    private const string levelgems = "gems";
    private const string money = "money";
    private const string player = "Player";
    private const string skates = "Skates";
    private const string music = "Music";
    private const string vibratation = "Vibratation";
    private const string charackterSellMoney = "CharackerSellMoney";
    private const string patentSellMoney = "PatentSellMoney";
    private const string woman0 = "Woman0";
    private const string patent0 = "Patent0";
    private const string level = "level";
    private const string DressCount = "DressCount";
    private const string PatentCount = "PatentCount";
    private const string CurrentLevel = "CurrentLevel";
    #endregion
    private void Awake()
    {
        hasKeyPlayerData(player, PlayerPrefEnum.Player, 0);
        hasKeyPlayerData(skates, PlayerPrefEnum.Skates, 0);
        hasKeyPlayerData(patentSellMoney, PlayerPrefEnum.PatentSellMoney, 500);
        HasKeyPlayerPref(speed, 300);
        HasKeyPlayerPref(speedController, 10);
        HasKeyPlayerPref(levelSpeed + level, 1);
        HasKeyPlayerPref(levelgems + level, 1);
        HasKeyPlayerPref(levelgems, 150);
        hasKeyPlayerData(money, PlayerPrefEnum.money, 0);
        hasKeyPlayerData(music, PlayerPrefEnum.Music, 1);
        hasKeyPlayerData(vibratation, PlayerPrefEnum.Vibratation, 1);
        hasKeyPlayerData(charackterSellMoney, PlayerPrefEnum.CharackerSellMoney, 500);
        hasKeyPlayerData(woman0, PlayerPrefEnum.Woman0, 2);
        hasKeyPlayerData(patent0, PlayerPrefEnum.Patent0, 2);
    }
    void Start()
    {
        GetDataInt(PlayerPrefEnum.Player);
    }
    public int GetDataInt(PlayerPrefEnum playerDataEnum) => (playerDataEnum) switch
    {
        PlayerPrefEnum.Player => GetPlayerPrefInt(player),
        PlayerPrefEnum.Skates => GetPlayerPrefInt(skates),
        PlayerPrefEnum.money => GetPlayerPrefInt(money),
        PlayerPrefEnum.CharackerSellMoney => GetPlayerPrefInt(charackterSellMoney),
        PlayerPrefEnum.PatentSellMoney => GetPlayerPrefInt(patentSellMoney),
        PlayerPrefEnum.Patent0 => GetPlayerPrefInt(patent0),
        PlayerPrefEnum.Woman0 => GetPlayerPrefInt(woman0),
        PlayerPrefEnum.DressCount => GetPlayerPrefInt(DressCount),
        PlayerPrefEnum.PatentCount => GetPlayerPrefInt(PatentCount),
        PlayerPrefEnum.Music => GetPlayerPrefInt(music),
        PlayerPrefEnum.Vibratation => GetPlayerPrefInt(vibratation),
        PlayerPrefEnum.CurrentLevel => GetPlayerPrefInt(CurrentLevel),

        _ => 0
    };
    public void SetDataInt(PlayerPrefEnum playerDataEnum, int value)
    {
        switch (playerDataEnum)
        {
            case PlayerPrefEnum.Player:
                SetPlayerPrefInt(player, value);
                break;
            case PlayerPrefEnum.Skates:
                SetPlayerPrefInt(skates, value);
                break;
            case PlayerPrefEnum.money:
                SetPlayerPrefInt(money, value);
                break;
            case PlayerPrefEnum.CharackerSellMoney:
                SetPlayerPrefInt(charackterSellMoney, value);
                break;
            case PlayerPrefEnum.PatentSellMoney:
                SetPlayerPrefInt(patentSellMoney, value);
                break;
            case PlayerPrefEnum.Patent0:
                SetPlayerPrefInt(patent0, value);
                break;
            case PlayerPrefEnum.Woman0:
                SetPlayerPrefInt(woman0, value);
                break;
            case PlayerPrefEnum.DressCount:
                SetPlayerPrefInt(DressCount, value);
                break;
            case PlayerPrefEnum.PatentCount:
                SetPlayerPrefInt(PatentCount, value);
                break;
            case PlayerPrefEnum.Music:
                SetPlayerPrefInt(music, value);
                break;
            case PlayerPrefEnum.Vibratation:
                SetPlayerPrefInt(vibratation, value);
                break;
            case PlayerPrefEnum.CurrentLevel:
                SetPlayerPrefInt(CurrentLevel, value);
                break;
            default:
                break;
        }
    }
    int GetPlayerPrefInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }
    void SetPlayerPrefInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
    public bool hasKeyPlayerData(string name, PlayerPrefEnum space, int value)
    {
        if (PlayerPrefs.HasKey(name))
            return true;
        else
        {
            SetDataInt(space, value);
            return true;
        }
    }
    public int GetMoneyUser()
    {
        return GetPlayerPrefInt(money);
    }
    bool HasKeyPlayerPref(string name, int value)
    {
        if (PlayerPrefs.HasKey(name))
            return true;
        else
        {
            PlayerPrefs.SetInt(name, value);
            return true;
        }
    }

}
