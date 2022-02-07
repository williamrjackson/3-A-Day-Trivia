using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class QuestionServer : MonoBehaviour
{
    public TextAsset filmCsv;
    public TextAsset musicCsv;
    public TextAsset miscCsv;

    private static QuestionServer _instance;
    private List<Dictionary<string, object>> filmQuestions;
    private List<Dictionary<string, object>> musicQuestions;
    private List<Dictionary<string, object>> miscQuestions;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if (filmCsv != null)
                filmQuestions = CSVReader.Read(filmCsv);
            if (musicCsv != null)
                musicQuestions = CSVReader.Read(musicCsv);
            if (miscCsv != null)
                miscQuestions = CSVReader.Read(miscCsv);
        }
        else
        {
            Destroy(this);
        }
    }

    public static QuestionServer Instance => _instance;

    public Question GetQuestion(int type)
    {
        // TEMPORARY:
        type = 0;

        List<Dictionary<string, object>> source;
        switch (type)
        {
            case 0:
                source = filmQuestions;
                break;
            case 1:
                source = musicQuestions;
                break;
            case 2:
                source = miscQuestions;
                break;
            default:
                source = filmQuestions;
                break;
        }
        Dictionary<string, object> rand = source[Random.Range(0, source.Count)];
        return new Question(rand["Question"].ToString(), rand["Answer"].ToString(), rand["Tolerance"].ToString());
    }
}

public class Question
{
    public string question;
    public string answer;
    public string tolerance;

    public Question(string question, string answer, string tolerance)
    {
        this.question = question;
        this.answer = answer;
        this.tolerance = tolerance;
    }

    public bool CheckAnswer(string submition)
    {
        submition = submition.ToLower().Trim();
        if (submition == this.answer.ToLower().Trim())
        {
            return true;
        }
        if (Regex.IsMatch(submition, WildcardToRegex(tolerance.ToLower().Trim())))
        {
            return true;
        }
        return false;
    }
    private static string WildcardToRegex(string value)
    {
        return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
    }

}
