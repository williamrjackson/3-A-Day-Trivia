using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public delegate void KeyboardLetterEvent(char letter);
    public KeyboardLetterEvent OnLetter;
    public UnityAction OnBackspace;
    public UnityAction OnEnter;
    private static List<KeyInteraction> keys;
    public static Keyboard Instance;
    void Awake ()
    {
        keys = new List<KeyInteraction>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple Keyboard's instantiated. Component removed from " + gameObject.name + ". Instance already found on " + Instance.gameObject.name + "!");
            Destroy(this);
        }
    }

    public void AddKey(KeyInteraction key)
    {
        if (keys == null)
        {
            keys = new List<KeyInteraction>();
        }
        if (keys.Contains(key))
        {
            return;
        }
        keys.Add(key);
    }
    public void RemoveKey(KeyInteraction key)
    {
        if (keys == null) return;
        if (keys.Contains(key))
        {
            keys.Remove(key);
        }
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            string inKey;
            if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
            {
                inKey = "+";
            }
            else if (e.keyCode == KeyCode.Backspace)
            {
                inKey = "-";
            }
            else
            {
                inKey = e.character.ToString().ToUpper();
            }
            foreach (var key in keys)
            {
                if (inKey == key.Character.ToString())
                {
                    key.MouseUp();
                }
            }
        }
    }
    internal void RegisterLetter(char character)
    {
        if (OnLetter != null)
        {
            OnLetter(character);
        }
    }

    internal void RegisterBackspace()
    {
        if (OnBackspace != null)
        {
            OnBackspace();
        }
    }
    internal void RegisterEnter()
    {
        if (OnEnter != null)
        {
            OnEnter();
        }
    }
}
