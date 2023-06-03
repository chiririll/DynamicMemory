using DG.Tweening;
using DynamicMem.Config;
using DynamicMem.Model;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class TaskItem : MonoBehaviour
    {
        [SerializeField] private RectTransform rt;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text memoryUsage;
        [SerializeField] private Slider progress;
        [SerializeField] private Image background;
        [SerializeField] private Button button;
        [Space]
        [SerializeField] private Color defaultBgColor;
        [SerializeField] private Color defaultTextColor;
        [SerializeField] private List<StateColor> stateColors;
        [SerializeField] private float shiftColorDuration;

        private readonly LazyInject<AppConfig> config = new();
        private ITask task;

        public IObservable<Unit> OnClick => button.OnClickAsObservable();

        public void SetData(ITask task)
        {
            this.task = task;

            title.text = task.Label;

            memoryUsage.text = task.Size.ToMemoryString();

            progress.maxValue = task.MaxLifetime;
            task.Lifetime.Subscribe(UpdateProgress);

            task.Status.Subscribe(OnStatusChanged);
            OnStatusChanged(task.Status.Value);
        }

        public void SetWidth(float width)
        {
            rt.DOSizeDelta(new Vector2(width, rt.sizeDelta.y), config.Value.simulation.TickTime / 2);
            //rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
        }

        public void UpdateProgress(int value)
        {
            if (task.Status.Value != Task.State.Running) 
                return;
            
            progress.value = value;
            //progress.DOValue(value + 1, config.Value.simulation.TickTime).SetEase(Ease.Linear);
        }

        private void OnStatusChanged(Task.State state)
        {
            UpdateProgress(task.Lifetime.Value);

            var bgColor = defaultBgColor;
            var textColor = defaultTextColor;

            foreach (var item in stateColors)
            {
                if (item.state != state) continue;

                bgColor = item.bgColor;
                textColor = item.textColor;
                break;
            }
            
            background.DOColor(bgColor, shiftColorDuration);
            title.DOColor(textColor, shiftColorDuration);
            memoryUsage.DOColor(textColor, shiftColorDuration);
        }

        [System.Serializable]
        public class StateColor
        {
            [field:SerializeField] public Task.State state { get; private set; }
            [field:SerializeField] public Color bgColor { get; private set; }
            [field:SerializeField] public Color textColor { get; private set; }
        }
    }
}
