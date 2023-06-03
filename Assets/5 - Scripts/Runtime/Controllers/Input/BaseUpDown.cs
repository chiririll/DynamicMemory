using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class BaseUpDown : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;

        public IObservable<string> OnEndEdit => inputField.onEndEdit.AsObservable();
        public IObservable<Unit> OnUpClick => upButton.OnClickAsObservable();
        public IObservable<Unit> OnDownClick => downButton.OnClickAsObservable();

        public string Text 
        {
            get => inputField.text; 
            set => inputField.text = value;
        }
    }
}
