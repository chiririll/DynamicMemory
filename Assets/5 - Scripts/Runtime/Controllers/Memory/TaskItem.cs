using Cysharp.Threading.Tasks.Triggers;
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
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Image background;
        [SerializeField] private Button button;
        [Space]
        [SerializeField] private Color defaultBgColor;
        [SerializeField] private Color defaultTextColor;
        [SerializeField] private List<StateColor> stateColors;
        
        private readonly LazyInject<AppConfig> config = new();
        private AnimationManager animManager;
        private ITask task;

        public IObservable<Unit> OnClick => button.OnClickAsObservable();

        public void SetData(ITask task, AnimationManager animManager)
        {
            this.task = task;
            this.animManager = animManager;

            title.text = task.Label;

            memoryUsage.text = task.Size.ToMemoryString();

            progress.gameObject.SetActive(false);
            progress.maxValue = task.MaxLifetime;
            task.Lifetime.Subscribe(UpdateProgress);

            task.Status.Subscribe(OnStatusChanged);
            OnStatusChanged(task.Status.Value);
        }

        public void SetWidth(float width)
        {
            if (animManager.Enabled)
            {
                rt.DOSizeDelta(new Vector2(width, rt.sizeDelta.y), animManager.TaskResizeTime);
            }
            else
            {
                rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
            }
        }

        public void UpdateProgress(int value)
        {
            if (task.Status.Value != Task.State.Running) 
                return;
            
            progressText.text = $"{value}/{progress.maxValue}";
            if (animManager.Enabled && value < progress.maxValue)
            {
                progress.DOValue(value, animManager.ProgressTime).SetEase(Ease.Linear);
            }
            else
            {
                progress.value = value;
            }
        }

        private void OnStatusChanged(Task.State state)
        {
            if (state == Task.State.Running)
            {
                progress.gameObject.SetActive(true);
            }
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
            
            if (animManager.Enabled)
            {
                background.DOColor(bgColor, animManager.ShiftColorTime);
                title.DOColor(textColor, animManager.ShiftColorTime);
                memoryUsage.DOColor(textColor, animManager.ShiftColorTime);
            }
            else
            {
                background.color = bgColor;
                title.color = textColor;
                memoryUsage.color = textColor;
            }
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
