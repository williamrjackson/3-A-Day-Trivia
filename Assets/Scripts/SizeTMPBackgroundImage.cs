using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class SizeTMPBackgroundImage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI sourceText;
    [SerializeField]
    private RectTransform background;
    [SerializeField]
    private Vector2 padding = new Vector2(40f, 20f);

    public string Text
    {
        get => sourceText.text;
        set
        {
            sourceText.SetText(value);
            ResizeBackground();
        }
    }

    private void Awake()
    {
        if (sourceText == null|| background == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing required assignments.", this);
            this.enabled = false;
        }
    }

    public void ResizeBackground()
    {
        Vector2 renderedHeight = sourceText.GetRenderedValues(true);
        renderedHeight.x += padding.x;
        renderedHeight.y += padding.y;
        background.sizeDelta = renderedHeight;
    }


#if UNITY_EDITOR
    // In Editor Setup and Updates

    private bool _needsSetUp = false;
    private bool _needsEditorResize = false;

    private void LateUpdate()
    {
        if (_needsSetUp)
        {
            ConfigureTextComponent(); 
            _needsSetUp = false;
        }
        if (_needsEditorResize)
        {
            ResizeBackground();
            _needsEditorResize = false;
        }
    }

    private void ConfigureTextComponent()
    {
        sourceText.transform.SetParent(background);
        sourceText.enableWordWrapping = true;
        sourceText.overflowMode = TextOverflowModes.Overflow;
        sourceText.rectTransform.anchoredPosition = Vector2.zero;
        sourceText.rectTransform.anchorMin = Vector2.one * .5f;
        sourceText.rectTransform.anchorMax = Vector2.one * .5f;
        sourceText.rectTransform.pivot = Vector2.one * .5f;
    }

    private void OnValidate()
    {
        if (sourceText != null || background != null)
        {
            _needsSetUp = true;
            _needsEditorResize = true;
        }
    }
#endif

}
