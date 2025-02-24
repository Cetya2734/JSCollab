using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePrinter : MonoBehaviour
{
    public static DialoguePrinter Instance { get; private set; }
    
    [SerializeField] private TMP_Text _dialogueTextMesh;
    
    public void PrintDialougeLine(string LineToPrint, float charSpeed, Action finishedCallback)
    {
        StartCoroutine(CO_PrintDialougeLine(LineToPrint, charSpeed, finishedCallback));
    }

    private IEnumerator CO_PrintDialougeLine(string LineToPrint, float charSpeed, Action finishedCallback)
    {
        _dialogueTextMesh.SetText(string.Empty);

        for (int i = 0; i < LineToPrint.Length; i++)
        {
            var character = LineToPrint[i];
            _dialogueTextMesh.SetText(_dialogueTextMesh.text + character);

            yield return new WaitForSeconds(charSpeed);
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _dialogueTextMesh.SetText(string.Empty);

        finishedCallback?.Invoke();
        EventBus.Instance.ResumeGameplay();
        yield return null;
    }

    private void Awake()
    {
        Instance = this;
    }
}
