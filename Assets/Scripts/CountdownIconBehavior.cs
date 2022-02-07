using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownIconBehavior : MonoBehaviour
{
    [SerializeField]
    Image _circle;
    [SerializeField]
    Image _countdownFill;
    [SerializeField]
    Image _overFill;

    List<Wrj.Utils.MapToCurve.Manipulation> manipList = new List<Wrj.Utils.MapToCurve.Manipulation>();
    private void Start()
    {
        GameManager.Instance.OnNewQuestion += StopTimers;
    }

    private void StopTimers()
    {
        foreach (var item in manipList)
        {
            if (item.IsRunning)
            {
                item.Stop();
            }
        }
        manipList.Clear();
    }

    public void BeginCountdown()
    {
        StartCoroutine(BeginCountdown_Coro());
    }

    private IEnumerator BeginCountdown_Coro()
    {
        var circle = Wrj.Utils.MapToCurve.Linear.FadeAlpha(_circle.transform, 1f, .5f);
        manipList.Add(circle);
        yield return circle.coroutine;
        var fill1 = Wrj.Utils.MapToCurve.Linear.ManipulateFloat((v) => _countdownFill.fillAmount = v, 0f, 1f, GameManager.Duration);
        manipList.Add(fill1);
        yield return fill1.coroutine;
        var fill2 = Wrj.Utils.MapToCurve.Linear.ManipulateFloat((v) => _overFill.fillAmount = v, 0f, 1f, GameManager.Duration);
        manipList.Add(fill2);
        yield return fill2.coroutine;
    }

}
