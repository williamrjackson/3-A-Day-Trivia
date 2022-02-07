using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
public class KeyInteraction : MonoBehaviour
{
    public enum KeyType {Letter, Backspace, Enter }
    [SerializeField]
    private KeyType keyType = KeyType.Letter;
    EventTrigger _trigger;
    TextMeshProUGUI _characterTmp;
    char _character;
    public char Character => _character;
    private Button _button;

    void Start()
    {
        _characterTmp = transform.GetComponentInChildren<TextMeshProUGUI>();
        if (keyType == KeyType.Enter)
        {
            _character = '+';
        }
        else if (keyType == KeyType.Backspace)
        {
            _character = '-';
        }
        else
        {
            _character = _characterTmp.text[0];
        }
        _button = GetComponent<Button>();
        _button.onClick.AddListener(MouseUp);
        Keyboard.Instance.AddKey(this);
    }

    private void MouseDown(BaseEventData arg0)
    {
        transform.localScale = Vector3.one * 1.5f;
    }

    public void MouseUp()
    {
        switch (keyType)
        {
            case KeyType.Letter:
                Keyboard.Instance.RegisterLetter(_character);
                break;
            case KeyType.Backspace:
                Keyboard.Instance.RegisterBackspace();
                break;
            case KeyType.Enter:
                Keyboard.Instance.RegisterEnter();
                break;
            default:
                break;
        }
        transform.localScale = Vector3.one;
    }
}
