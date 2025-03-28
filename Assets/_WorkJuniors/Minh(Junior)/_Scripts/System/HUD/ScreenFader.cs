// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System;
// using System.ComponentModel;

//
// public class ScreenFader : MonoBehaviour
// {
//     public static ScreenFader Instance { get; private set; }
//
//     [SerializeField] private Image _fader;
//
//     private bool _isBusy;
//
//     // ReSharper disable Unity.PerformanceAnalysis
//     public void FadeToBlack(float duration, Action finishedCallback)
//     {
//         if (_isBusy) return;
//         StartCoroutine((CO_FadeToBlack(duration, finishedCallback)));
//     }
//
//     public void FadeFromBlack(float duration, Action finishedCallback)
//     {
//         if (_isBusy) return;
//         StartCoroutine((CO_FadeFromBlack(duration, finishedCallback)));
//     }
//
//     private void Awake()
//     {
//         Instance = this;
//     }
//
//     private IEnumerator CO_FadeToBlack(float duration, Action finishedCallback)
//     {
//         _isBusy = true;
//
//         while (_fader.color.a < 1)
//         {
//             _fader.color = new Color(0, 0, 0, _fader.color.a + (10  / duration));
//             yield return null;
//         }
//         _fader.color = new Color(0, 0, 0, 1);
//         _isBusy = false;
//
//         finishedCallback?.Invoke();
//
//         yield return null;
//     }
//     private IEnumerator CO_FadeFromBlack(float duration, Action finishedCallback)
//     {
//         _isBusy = true;
//
//         while (_fader.color.a > 0)
//         {
//             _fader.color = new Color(0, 0, 0, _fader.color.a - (10 / duration));
//             yield return null;
//         }
//         _fader.color = new Color(0, 0, 0, 0);
//         _isBusy = false;
//
//         finishedCallback?.Invoke();
//
//         yield return null;
//     }
// }
//
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private Image _fader;

    private bool _isBusy;

    private void Awake()
    {
        Instance = this;
    }

    public void FadeToBlack(float duration, Action finishedCallback)
    {
        if (_isBusy) return;
        _isBusy = true;

        _fader.DOFade(1f, duration).SetUpdate(true).OnComplete(() =>
        {
            _isBusy = false;
            finishedCallback?.Invoke();
        });
    }

    public void FadeFromBlack(float duration, Action finishedCallback)
    {
        if (_isBusy) return;
        _isBusy = true;

        _fader.DOFade(0f, duration).SetUpdate(true).OnComplete(() =>
        {
            _isBusy = false;
            finishedCallback?.Invoke();
        });
    }
}

