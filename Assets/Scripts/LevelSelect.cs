using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private string levelBtnName;
    [SerializeField] private RectTransform btnHolder;
    [SerializeField] private ContentSizeFitter sizeFitter;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField][Range(0,1)] private float duration;
    
    private List<LevelBtn> _allBtns;
    
    public void Initialize(List<Level> levels)
    {
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
        GenerateLevels(levels);
        sizeFitter.enabled = false;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, duration);
    }

    private void Close(Action callback)
    {
        canvasGroup.DOFade(0, duration).OnComplete(() =>
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        });
    }
    
    private void GenerateLevels(List<Level> levels)
    {
        _allBtns = new List<LevelBtn>();
        var index = 0;
        foreach (var lev in levels)
        {
            var level = PoolManager.Pools[GameController.Instance.poolName].Spawn(this.levelBtnName, this.btnHolder)
                .gameObject.GetComponent<LevelBtn>();
            level.Initialize(this,index);
            index++;
            _allBtns.Add(level);
        }
        sizeFitter.enabled = true;
    }

    public void OnSelectLevel(int index)
    {
        Close(() =>
        {
            GameController.Instance.StartGame(index);
        });
    }
}
