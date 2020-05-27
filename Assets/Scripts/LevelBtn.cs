using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    [SerializeField] private Text indexTxt;
    private int _levelIndex;
    private LevelSelect _parent;
    
    public void Initialize(LevelSelect parent, int levelIndex)
    {
        _parent = parent;
        _levelIndex = levelIndex;
        SetIndex(levelIndex);
        GetComponent<Button>().onClick.AddListener(PlayLevel);
    }

    private void SetIndex(int index)
    {
        indexTxt.text = index >= 10 ? (index + 1).ToString() : "0" + (index + 1);
    }

    private void PlayLevel()
    {
        if(_parent == null) return;
        AudioManager.Instance.Play(SoundsList.SelectLevel);
        _parent.OnSelectLevel(this._levelIndex);
    }
}
