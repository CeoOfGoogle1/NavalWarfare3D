using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class MultiToggleButton : MonoBehaviour
{
    [Serializable]
    public class State
    {
        public string text;
        public Color backgroundColor = Color.white;
        public Color textColor = Color.black;
        public UnityEvent onSelected;
    }

    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text label;

    [Header("States")]
    [SerializeField] private State[] states;

    private int currentState;

    public int CurrentState => currentState;

    private void Awake()
    {
        button.onClick.AddListener(NextState);
        ApplyState();
    }

    public void NextState()
    {
        currentState = (currentState + 1) % states.Length;
        ApplyState();
    }

    public void SetState(int index)
    {
        currentState = Mathf.Clamp(index, 0, states.Length - 1);
        ApplyState();
    }

    private void ApplyState()
    {
        if (states == null || states.Length == 0) return;

        State state = states[currentState];

        label.text = state.text;
        label.color = state.textColor;
        background.color = state.backgroundColor;
        state.onSelected?.Invoke();
    }
}