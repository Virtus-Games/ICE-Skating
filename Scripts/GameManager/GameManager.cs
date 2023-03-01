using UnityEngine;

[System.Serializable]
public enum SoundsType
{
    START,
    COLLECT,
    WIN,
    ClICK,
    OVER,
}


public class GameManager : MonoBehaviour
{

    public static GameManager Current;
    private SettingsManager settingsManager;

    [Header("Audio Manager")]
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource CamAudioPlayShot;
    [SerializeField] private AudioSource music;

    [Header("Level Slider")]
    public int score = 0;

    [Header("Player And Finish")]
    [SerializeField] private Transform Player;
    public int passleAtFinishLine = 0;
    [SerializeField] private Juri[] juries;

    float overTime = 2;

    private CharacterControl cc;
    private bool isStarted = false;



    private void Awake()
    {
        Current = this;
        settingsManager = GetComponent<SettingsManager>();
        music = GameObject.FindWithTag("audio").GetComponent<AudioSource>();


    }

    public void Run()
    {
        Player = GameObject.FindWithTag("Player").transform;
        Player.GetComponent<AnimatorController>().RunPlayer();
        isStarted = true;

    }

    public void FinishController(int value)
    {
        passleAtFinishLine += value;
    }

    public void ChangeScore(int increment)
    {
        music = GameObject.FindWithTag("audio").GetComponent<AudioSource>();
        if (settingsManager.GetSounds() == 1)
            ControlSounds(SoundsType.COLLECT);

        if (settingsManager.GetVibration() == 1)
            Vibrator.Vibrate(100);

        score += increment;
    }

    public int GetScore()
    {
        // if (score <= 0)
        //     return 0;
        // else
        //     return score;

        return Mathf.Max(0, score);
    }

    #region Audio Controller
    public void ControlSounds(SoundsType sounds)
    {
        switch (sounds)
        {
            case SoundsType.START:
                break;
            case SoundsType.COLLECT:
                AudioControllerShot(clips[0]);
                break;
            case SoundsType.WIN:
                AudioControllerShot(clips[4]);
                break;
            case SoundsType.OVER:

                break;
            case SoundsType.ClICK:
                AudioControllerShot(clips[1]);
                break;
        }
    }

    public void AudioControllerShot(AudioClip clip)
    {
        CamAudioPlayShot.PlayOneShot(clip);
    }

    #endregion

    private void Update()
    {
        if (isStarted)
        {
            if (Player.transform.position.y < -3)
            {
                overTime -= Time.deltaTime;

                if (overTime < 0)
                    GetComponent<LevelManager>().Over();
            }
        }
    }



    [ContextMenu("Juri")]
    public void Juri()
    {
        JuriController();
    }

    public void JuriController()
    {
        cc = Player.GetComponent<CharacterControl>();

        foreach (Juri juri in juries)
        {
            juri.Canvas.SetActive(true);
            juri.m_Anim.SetBool("isJuri", true);
            juri.ScoreText.text =
                    (JuriScoreController()).ToString();
        }
    }

    int rand;
    int JuriScoreController()
    {
        if (cc.StatusImage() >= 0.5f)
            rand = Random.Range(5, 10);
        else
            rand = Random.Range(1, 6);
        return rand;
    }
}
