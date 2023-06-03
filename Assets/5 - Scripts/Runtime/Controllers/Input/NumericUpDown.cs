using System;
using UniRx;
using UnityEngine;

namespace DynamicMem
{
    public abstract class NumericUpDown<T> : MonoBehaviour where T : struct, IComparable<T>, IEquatable<T>
    {
        [SerializeField] private BaseUpDown upDown;
        [Header("Settings")]
        [SerializeField] private T step;
        [SerializeField] private T defaultValue;
        [Space(5)]
        [SerializeField] private bool limitMinValue;
        [SerializeField] private T minValue;
        [Space(2)]
        [SerializeField] private bool limitMaxValue;
        [SerializeField] private T maxValue;

        private readonly Subject<T> onValueChanged = new();


        private T value;

        public IObservable<T> OnValueChanged => onValueChanged;
        public T Value
        {
            get => value;
            private set => SetValue(value, true);
        }

        private void Awake()
        {
            value = Clamp(defaultValue);
            upDown.Text = Value.ToString();

            upDown.OnEndEdit.Subscribe(_ => CheckValue()).AddTo(this);
            upDown.OnUpClick.Subscribe(_ => IncrementValue()).AddTo(this);
            upDown.OnDownClick.Subscribe(_ => DecrementValue()).AddTo(this);
        }

        public void SetValue(T value, bool raiseEvent = true)
        {
            var trueVal = Clamp(value);

            if (trueVal.Equals(this.value))
            {
                return;
            }

            this.value = trueVal;
            upDown.Text = this.value.ToString();

            if (raiseEvent)
            {
                onValueChanged?.OnNext(this.value);
            }
        }

        private void CheckValue()
        {
            if (!TryGetInput(out var value))
            {
                return;
            }
            Value = value;
        }

        private void IncrementValue()
        {
            Value = Increment(Value, step);
        }

        private void DecrementValue()
        {
            Value = Decrement(Value, step);
        }

        private bool TryGetInput(out T value)
        {
            if (!TryParse(upDown.Text, out value))
            {
                upDown.Text = this.Value.ToString();
                return false;
            }
            return true;
        }

        private T Clamp(T value)
        {
            if (limitMaxValue && value.CompareTo(maxValue) > 0)
                return maxValue;
            if (limitMinValue && value.CompareTo(minValue) < 0)
                return minValue;
            return value;
        }

        protected abstract bool TryParse(string text, out T value);
        protected abstract T Increment(T value, T step);
        protected abstract T Decrement(T value, T step);
    }
}