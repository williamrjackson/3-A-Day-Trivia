using UnityEngine;

public class ResultsView : MonoBehaviour
{
    public TMPro.TextMeshProUGUI resultsText;

    private static ResultsView _instance;
    public static ResultsView Instance => _instance;

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

    public void FillResults(string results)
    {
        resultsText.SetText(results);
    }
}
