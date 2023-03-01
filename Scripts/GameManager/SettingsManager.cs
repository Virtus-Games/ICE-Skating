using UnityEngine;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    private PlayerData playerData;

    [Header("Settings Audio")]
    [SerializeField] private ImageButton musicType;
    [SerializeField] private ImageButton vibratationType;
    [SerializeField] private Image musicImage;
    [SerializeField] private Image vibratationImage;

    private AudioSource music;
    private int _statusMusic;
    private int _statusVibrat;

    void Start()
    {

        GetMusicValue();
        SetImage(_statusMusic, musicImage, musicType);
        SetImage(_statusVibrat, vibratationImage, vibratationType);
        music = GameObject.FindWithTag("audio").GetComponent<AudioSource>();
        if(_statusMusic == 1){
            music.Play();
        } else
            music.Stop();
    }

    private void OnEnable()
    {
        playerData = FindObjectOfType<PlayerData>();
    }


    #region Settings Audio Controller

    void GetMusicValue()
    {
        _statusMusic = playerData.GetDataInt(PlayerPrefEnum.Music);
        _statusVibrat = playerData.GetDataInt(PlayerPrefEnum.Vibratation);
    }

    // "Button Controller"
    public void SettingsController(int type)
    {

        GetMusicValue();
        music = GameObject.FindWithTag("audio").GetComponent<AudioSource>();

        if (type == 1)
        {
            ImageStatusController(_statusMusic, PlayerPrefEnum.Music, musicImage, musicType);
            // music.Play();
        }
        else
            ImageStatusController(_statusVibrat, PlayerPrefEnum.Vibratation, vibratationImage, vibratationType);
    }

    void ImageStatusController(int status, PlayerPrefEnum pref, Image image, ImageButton img)
    {
        if (status == 0)
        {
            playerData.SetDataInt(pref, 1);
            if (pref == PlayerPrefEnum.Music)
                music.Play();
        }
        else
        {
            playerData.SetDataInt(pref, 0);
            if (pref == PlayerPrefEnum.Music)
                music.Stop();
        }


        status = playerData.GetDataInt(pref);
        SetImage(status, image, img);
    }

    public int GetVibration()
    {
        GetMusicValue();
        return _statusVibrat;
    }

    public int GetSounds()
    {
        GetMusicValue();
        return _statusMusic;
    }
    void SetImage(int status, Image component, ImageButton imageType)
    {
        if (status == 1)
        {
            component.sprite = imageType.OpenSprite;

        }
        else
            component.sprite = imageType.CloseSprite;

    }

    #endregion
}
