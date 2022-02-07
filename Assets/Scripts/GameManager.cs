using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public CanvasFade mainScreen;
    public CanvasFade questionScreen;
    public CanvasFade resultsScreen;
    public TextMeshProUGUI question;
    public CountdownIconBehavior[] icons;

    [SerializeField]
    private float questionDuration = 10f;

    private Tuple<Question, string, float>[] questionsAndAnswers = new Tuple<Question, string, float>[3];

    public UnityAction OnNewQuestion;
    private int _currentQuestion = -1;
    public int CurrentQuestion => _currentQuestion;
    public bool IsStartScreen => CurrentQuestion < 0;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static float Duration => Instance.questionDuration;
    private void Start()
    {
        _currentQuestion = -1;
        mainScreen.IsActive = true;
        questionScreen.IsActive = false;
        resultsScreen.IsActive = false;
    }

    public void StartGame()
    {
        if (IsStartScreen)
        {
            _currentQuestion = 0;
            StartCoroutine(TransitionToGame());
        }
    }

    public void ProgressQuestion(Question q, string answer, float time)
    {
        questionsAndAnswers[_currentQuestion] = new Tuple<Question, string, float>(q, answer, time);
        if (OnNewQuestion != null)
        {
            OnNewQuestion();
        }
        _currentQuestion++;
        if (_currentQuestion > 2)
        {
            _currentQuestion = -1;
            string results = "";
            for (int i = 0; i < questionsAndAnswers.Length; i++)
            {
                Tuple<Question, string, float> item = questionsAndAnswers[i];
                bool correct = item.Item1.CheckAnswer(item.Item2);
                TimeSpan duration = TimeSpan.FromSeconds(item.Item3);
                string strDuration = duration.ToString(@"m\:ss");
                if (correct)
                {
                    results += ($"Question {i + 1}: Correct!\n{item.Item1.answer}\nTime: {strDuration}");
                }
                else
                {
                    results += ($"Question {i + 1}: Wrong!\n{item.Item1.answer} vs. {item.Item2}");
                }
                if (i < questionsAndAnswers.Length - 1)
                {
                    results += "\n\n";
                }
            }
            ResultsView.Instance.FillResults(results);
            StartCoroutine(ShowResults_Coro());
            return;
        }
        StartCoroutine(NextQuestion_Coro());
    }

    IEnumerator NextQuestion_Coro()
    {
        yield return questionScreen.SetActive(false);
        Question question = QuestionServer.Instance.GetQuestion(_currentQuestion);
        QuestionView.Instance.LoadQuestion(question);
        yield return questionScreen.SetActive(true);

        icons[_currentQuestion].BeginCountdown();
    }

    IEnumerator TransitionToGame()
    {
        yield return mainScreen.SetActive(false);
        Question question = QuestionServer.Instance.GetQuestion(_currentQuestion);
        QuestionView.Instance.LoadQuestion(question);
        yield return questionScreen.SetActive(true);
        icons[0].BeginCountdown();
    }

    IEnumerator ShowResults_Coro()
    {
        yield return questionScreen.SetActive(false);
        yield return resultsScreen.SetActive(true);
    }
}
