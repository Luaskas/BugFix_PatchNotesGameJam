using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Debugging
{
    using UnityEngine;
    using TMPro;
    using System.Collections.Generic;
    using System.Linq;

    public class OnScreenDebugController : MonoBehaviour
    {
        public static OnScreenDebugController Instance { get; private set; }

        [SerializeField] private TMP_Text debugText;

        private readonly Dictionary<string, DebugLine> lines = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public DebugLine CreateLine(string key, string initialText = "")
        {
            if (lines.ContainsKey(key))
                return lines[key];

            var line = new DebugLine(key, initialText);
            line.OnTextChanged += _ => UpdateText(); // auto-refresh when edited
            lines[key] = line;
            UpdateText();
            return line;
        }

        public void RemoveLine(string key)
        {
            if (lines.TryGetValue(key, out var line))
            {
                line.OnTextChanged -= _ => UpdateText();
                lines.Remove(key);
                UpdateText();
            }
        }

        private void UpdateText()
        {
            debugText.text = string.Join("\n", lines.Values.Select(l => l.Text));
        }
    }
    
    public class DebugLine
    {
        public string Key { get; private set; }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                if (text == value) return; // No change, skip

                text = value;
                OnTextChanged?.Invoke(this); // Notify controller
            }
        }

        // Event triggered when text changes
        public event Action<DebugLine> OnTextChanged;

        public DebugLine(string key, string initialText = "")
        {
            Key = key;
            text = initialText;
        }
    }

}