using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private Image background;
    [SerializeField] private GameObject loader;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Color initColor, fadeColor;
    [SerializeField] private bool isStartGame;

    public static bool IS_LOADED_GAME = false;

    #region SINGLETON
    public static SceneLoader Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Initialize();
            return;
        }
        Destroy(gameObject);
    }
    #endregion

    private void Initialize()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (isStartGame)
        {
            LoadScene(1);
        }
    }

    public async void LoadScene(int sceneID)
    {
        var scene = SceneManager.LoadSceneAsync(sceneID);
        scene.allowSceneActivation = false;

        loadingCanvas.worldCamera = Camera.main;
        loader.SetActive(true);
        background.gameObject.SetActive(true);
        background.color = initColor;
        loadingBar.value = 0;

        float virtualProgress = 0;
        do
        {
            await UniTask.Delay(100);
            loadingBar.value = virtualProgress / 0.9f;
            virtualProgress += (scene.progress - virtualProgress) * 0.8f;
        } while (virtualProgress < 0.9f);

        scene.allowSceneActivation = true;
        await UniTask.WaitUntil(() => scene.progress == 1);

        loader.SetActive(false);
        background.DOColor(fadeColor, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
        {
            background.gameObject.SetActive(false);
        });

        IS_LOADED_GAME = true;
    }

    public async void LoadSceneWithoutLoadingCutScreen(int sceneID)
    {
        var scene = SceneManager.LoadSceneAsync(sceneID);
        scene.allowSceneActivation = false;
        loadingCanvas.worldCamera = Camera.main;
        loader.SetActive(false);
        background.gameObject.SetActive(true);
        background.color = Color.clear;
        background.DOColor(Color.white, 0.05f).SetEase(Ease.InFlash);
        await UniTask.Delay(50);
        do
        {
            await UniTask.Delay(1);
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        await UniTask.WaitUntil(() => scene.progress == 1);

        background.DOColor(fadeColor, 1f).SetEase(Ease.OutFlash).OnComplete(() =>
        {
            background.gameObject.SetActive(false);
        });
    }
}
