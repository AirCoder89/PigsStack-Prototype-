using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
   public delegate void CounterEvents();
   public static event CounterEvents OnComplete;
   
   [SerializeField] private Image timeProgress;
   [SerializeField] private Text timeTxt;

   private float _targetTime;
   private bool _isCounting;
   private float _currentTime;
   
   [Button("Start Counting", ButtonSizes.Medium)]
   public void StartCounting(float time)
   {
      Open();
      this._targetTime = time;
      _currentTime = 0f;
      _isCounting = true;
   }

   public void StopCounting()
   {
      _isCounting = false;
      Close();
   }
   
   private void Update()
   {
      if(!_isCounting) return;
      if (_currentTime >= _targetTime)
      {
         _isCounting =false;
         OnComplete?.Invoke();
         Close();
         return;
      }

      _currentTime += Time.deltaTime;
      UpdateTimeProgress();
   }

   private void UpdateTimeProgress()
   {
      var t = (int) _currentTime;
      timeTxt.text = t < 10 ? "0" + t : t.ToString();

      var progression = _currentTime / _targetTime;
      timeProgress.fillAmount = progression;
   }

   private void Open()
   {
      gameObject.SetActive(true);
   }

   private void Close()
   {
      gameObject.SetActive(false);
   }
   
}
