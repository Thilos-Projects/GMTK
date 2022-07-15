using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    static InputManager instance;
    public static InputManager get()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public struct KeyEvent
    {
        public string name;
        public UnityEvent eu;
        public UnityEvent ed;
        public UnityEvent e;
        public KeyCode key;
    }
    public List<KeyEvent> keyEvents;

    void Update()
    {
        for(int i = 0; i < keyEvents.Count; i++)
        {
            KeyEvent e = keyEvents[i];
            if (Input.GetKey(e.key))
                e.e.Invoke();
            if (Input.GetKeyUp(e.key))
                e.eu.Invoke();
            if (Input.GetKeyDown(e.key))
                e.ed.Invoke();
        }
    }

    public void requestInput(string name, UnityAction action, bool onDown, bool onUp, bool perFrame)
    {
        for (int i = 0; i < keyEvents.Count; i++)
        {
            KeyEvent e = keyEvents[i];
            if (e.name == name)
            {
                if (onDown)
                    e.ed.AddListener(action);
                if (onUp)
                    e.eu.AddListener(action);
                if (perFrame)
                    e.e.AddListener(action);
            }
        }
    }
}