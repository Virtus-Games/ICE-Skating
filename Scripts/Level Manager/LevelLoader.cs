using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public static LevelLoader Current;
    private const string _level = "Level ";
    private const string CurrentLevel = "CurrentLevel";
    private const string SceneLevelName = "SceneLevelSize";
    private const string LevelName = "Level";
    private const string MaterialDataName = "MaterialData";
    private int LevelInt = 0;
    private Scene _lastLoadedScene;
    [SerializeField] private int SceneLevelSizeInt;
    private int CurrentLevelInt = 0;
    private int materialInt = 0;

    [SerializeField] private Material[] Skyboxes;

    private void Awake()
    {
        FindObjectOfType<AdmobManager>().InitiliazedAds();

        if (!PlayerPrefs.HasKey(SceneLevelName))
            PlayerPrefs.SetInt(SceneLevelName, SceneLevelSizeInt);

        if (!PlayerPrefs.HasKey(LevelName))
            PlayerPrefs.SetInt(LevelName, 1);

        if (!PlayerPrefs.HasKey(MaterialDataName))
            PlayerPrefs.SetInt(MaterialDataName, 0);


        LevelInt = PlayerPrefs.GetInt(LevelName);
        SceneLevelSizeInt = PlayerPrefs.GetInt(SceneLevelName);

        SkyboxController();

        if (!PlayerPrefs.HasKey(CurrentLevel))
            PlayerPrefs.SetInt(CurrentLevel, 1);

        if (Current == null)
        {
            Current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);



    }
    void Start()
    {
        LevelController(_level + (PlayerPrefs.GetInt(CurrentLevel)).ToString());
    }

    public void LevelController(string name)
    {
        SkyboxController();
        CurrentLevelInt = PlayerPrefs.GetInt(CurrentLevel);

        if (CurrentLevelInt >= SceneLevelSizeInt)
        {
            PlayerPrefs.SetInt(CurrentLevel, 1);
            CurrentLevelInt = PlayerPrefs.GetInt(CurrentLevel);
            ChangeScene("Level " + (CurrentLevelInt).ToString());
        }
        else
            ChangeScene(name);
    }

    private void SkyboxController()
    {

        materialInt = PlayerPrefs.GetInt(MaterialDataName);
        LevelInt = PlayerPrefs.GetInt(LevelName);

        if ((LevelInt % 10) == 0)
        {
            materialInt += 1;
            PlayerPrefs.SetInt(MaterialDataName, materialInt);
        }

        materialInt = PlayerPrefs.GetInt(MaterialDataName);

        if (materialInt > (Skyboxes.Length - 1))
        {
            materialInt = 0;
            PlayerPrefs.SetInt(MaterialDataName, materialInt);
        }

        materialInt = PlayerPrefs.GetInt(MaterialDataName);
        RenderSettings.skybox = Skyboxes[materialInt];
    }

    void ChangeScene(string sceneName)
    {

        StartCoroutine(SceneController(sceneName));
    }

    IEnumerator SceneController(string sceneName)
    {
        if (_lastLoadedScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(_lastLoadedScene);
            bool isUnloadScene = false;
            while (!isUnloadScene)
            {
                isUnloadScene = !_lastLoadedScene.IsValid();
                yield return new WaitForEndOfFrame();

            }
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        bool isSceneLoaded = false;

        while (!isSceneLoaded)
        {
            _lastLoadedScene = SceneManager.GetSceneByName(sceneName);
            isSceneLoaded = _lastLoadedScene != null && _lastLoadedScene.isLoaded;
            yield return new WaitForEndOfFrame();
        }

    }


}
