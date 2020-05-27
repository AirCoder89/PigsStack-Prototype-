using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BlockGenerator))]
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    [TabGroup("Initialize")]public GameObject tank;
    [TabGroup("Initialize")]public string poolName;
    [TabGroup("Initialize")][SerializeField] private float firstScale = 0.1f;
    [TabGroup("Initialize")][SerializeField] private float timeToWin = 10f;
    [TabGroup("Initialize")][SerializeField] private CounterScript counter;
    [TabGroup("Initialize")][SerializeField] private LevelSelect levelSelect;
    [TabGroup("Initialize")][Title("Popups")][SerializeField] private UIPanel levelCompletePopup;
    [TabGroup("Initialize")][SerializeField] private UIPanel gameOverPopup;
    [TabGroup("Levels")][SerializeField] private List<Level> levels;
    private BlockGenerator _generator;
    private bool _isGameStarted;
    private int _currentLevel;
    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        _currentLevel = 0;
        CounterScript.OnComplete += OnCountComplete;
        _generator = GetComponent<BlockGenerator>();
        _generator.Initialize(firstScale);
        levelSelect.Initialize(this.levels);
    }

    public void OpenSelectLevel()
    {
        tank.SetActive(false);
        levelSelect.Open();
    }

    private void OnDestroy()
    {
        FallingDetector.OnDetectBlock -= OnBlockFall;
        CounterScript.OnComplete -= OnCountComplete;
    }

    private void OnCountComplete()
    {
        //level win
        AudioManager.Instance.Play(SoundsList.LevelWin);
        _isGameStarted = false;
        tank.SetActive(false);
        print("Level Win");
        levelCompletePopup.OpenPanel();
    }
    
    private void OnBlockFall()
    {
        FallingDetector.OnDetectBlock -= OnBlockFall;
        _isGameStarted = false;
        AudioManager.Instance.Play(SoundsList.GameOver);
        //game over
        print("You Lose");
        counter.StopCounting();
        gameOverPopup.OpenPanel();
    }

    [Button("Start Game",ButtonSizes.Medium)]
    public void StartGame(int levelIndex = 0)
    {
        if(_isGameStarted) return;
        if(levelIndex >= levels.Count) return;
        _currentLevel = levelIndex;
        FallingDetector.OnDetectBlock += OnBlockFall;
        _generator.Generate(levels[levelIndex]);
        _isGameStarted = true;
    }

    private void Update()
    {
        if(!_isGameStarted) return;
        if (Input.GetMouseButtonDown(0))
        {
            _generator.ProvideBlock();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _generator.ReleaseBlock();
        }
    }

    public void StartCounting()
    {
        this.counter.StartCounting(timeToWin);
    }

    public void RepeatLevel()
    {
        StartGame(_currentLevel);
    }

    public void NextLevel()
    {
        _currentLevel += 1;
        StartGame(_currentLevel);
    }

}
