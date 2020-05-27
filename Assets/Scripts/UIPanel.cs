using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [TabGroup("Animations")][SerializeField] [Range(0, 5)] private float duration;
    [TabGroup("Animations")][SerializeField] private Ease ease;
    [TabGroup("Animations")][SerializeField] private Vector2 startScale = Vector2.one;
    [TabGroup("Animations")][SerializeField] private Vector2 startPosition = Vector2.zero;
    
    [TabGroup("Events Handler")]
    [Title("On Open/Close Events")] 
    public UnityEvent onOpenPanel;
    [TabGroup("Events Handler")]public UnityEvent onClosePanel;
    
    [TabGroup("Events Handler")]
    [Title("On Click Events")]
    public UnityEvent onClickBtnA;
    [TabGroup("Events Handler")]public UnityEvent onClickBtnB;
    
    
    [TabGroup("Elements")] [SerializeField] private Image background;
    [TabGroup("Elements")] [SerializeField] private Button buttonA;
    [TabGroup("Elements")] [SerializeField] private Button buttonB;
    [TabGroup("Elements")] [SerializeField] private RectTransform panel;
    [TabGroup("Elements")] [SerializeField] private CanvasGroup canvasGroup;
    
    
    void Start()
    {
        if(buttonA) buttonA.onClick.AddListener(() =>
        {
            AudioManager.Instance.Play(SoundsList.Click);
            ClosePanel(onClickBtnA);
        });
        if(buttonB) buttonB.onClick.AddListener(() =>
        {
            AudioManager.Instance.Play(SoundsList.Click);
            ClosePanel(onClickBtnB);
        });
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        background.fillAmount = 0;
        panel.anchoredPosition = this.startPosition;
        panel.localScale = startScale;

        canvasGroup.DOFade(1, duration);
        background.DOFillAmount(1, duration).SetEase(ease);
        panel.DOAnchorPos(Vector2.zero, duration).SetEase(ease);
        panel.DOScale(Vector3.one, duration).SetEase(ease).OnComplete(() => { onOpenPanel?.Invoke(); });
    }

    private void ClosePanel(UnityEvent callback)
    {
        canvasGroup.DOFade(0, duration);
        background.DOFillAmount(0, duration).SetEase(ease);
        panel.DOAnchorPos(startPosition, duration).SetEase(ease);
        panel.DOScale(startScale, duration).SetEase(ease).OnComplete(() =>
        {
            callback?.Invoke();
            gameObject.SetActive(false);
            onClosePanel?.Invoke();
        });
    }
    
}
