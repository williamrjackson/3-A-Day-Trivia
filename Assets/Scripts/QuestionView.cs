﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(CanvasFade))]
public class QuestionView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI questionText;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    Button doneButton;
    [SerializeField]
    CanvasFade canvasFade;

    private Question currentQuestion;
    private float startTime = 0f;
    private static QuestionView _instance;
    public static QuestionView Instance => _instance;

    private void Awake()
    {
        if (canvasFade == null)
        {
            canvasFade = GetComponent<CanvasFade>();
        }
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        canvasFade.OnActivate += Activate;
    }

    private void Activate()
    {
        inputField.SetTextWithoutNotify(string.Empty);
        doneButton.interactable = false;
        Wrj.Utils.DeferredExecution(.25f, () =>
        {
            inputField.Select();
            inputField.ActivateInputField();
            startTime = Time.time;
        });
    }

    public void LoadQuestion(Question question)
    {
        currentQuestion = question;
        questionText.SetText(currentQuestion.question);
    }

    public void SubmitAnswer()
    {
        GameManager.Instance.ProgressQuestion(currentQuestion, inputField.text, Time.time - startTime);
    }

    public void HandleRunButtonEnabledState(string text)
    {
        doneButton.interactable = !string.IsNullOrEmpty(text);
    }
}