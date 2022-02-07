using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool _isActive = false;

    public UnityEngine.UI.Button enterButton;

    public UnityAction OnActivate;
    public UnityAction OnDeactivate;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        _isActive = canvasGroup.alpha == 1f;
    }

    public Coroutine SetActive(bool active)
    {
        if (active == _isActive) return null;
        _isActive = active;
        AlertActivenessChange();
        return StartCoroutine(HandleCanvas(_isActive));
    }

    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            if (value == _isActive) return;
            _isActive = value;
            AlertActivenessChange();
            HandleCanvasImmediate(_isActive);
        }
    }

    private void AlertActivenessChange()
    {
        if (_isActive)
        {
            if (OnActivate != null)
            {
                OnActivate();
            }
        }
        else
        {
            if (OnDeactivate != null)
            {
                OnDeactivate();
            }
        }
    }

    private void HandleCanvasImmediate(bool setActive)
    {
        float targetVal = (setActive) ? 1f : 0f;
        canvasGroup.alpha = targetVal;
        canvasGroup.blocksRaycasts = setActive;
        canvasGroup.interactable = setActive;
    }

    private IEnumerator HandleCanvas(bool setActive)
    {
        if (setActive)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            yield return Wrj.Utils.MapToCurve.Linear.ManipulateFloat((v) => canvasGroup.alpha = v, 0f, 1f, .5f).coroutine;
        }
        else
        {
            yield return Wrj.Utils.MapToCurve.Linear.ManipulateFloat((v) => canvasGroup.alpha = v, 1f, 0f, .5f).coroutine;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    private void Update()
    {
        if (!Input.anyKey) return;
        if (canvasGroup.alpha == 1f && enterButton.interactable && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            enterButton.onClick.Invoke();
        }
    }
}
