using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainApp : MonoBehaviour
{
   [SerializeField] private RectTransform center;
   [SerializeField] private Image background;
   [SerializeField] private Button startBtn;
   [SerializeField] [Range(0, 5)] private float duration;
   [SerializeField] private Ease ease;
   private void Start()
   {
      this.startBtn.onClick.AddListener(OnClickStart);
   }

   private void OnClickStart()
   {
      AudioManager.Instance.Play(SoundsList.StartGame);
      center.DOScale(0, duration/2).SetEase(ease);
      background.DOFillAmount(0, duration).SetEase(ease).OnComplete(() =>
      {
         gameObject.SetActive(false);
         GameController.Instance.OpenSelectLevel();
      });
   }
}
