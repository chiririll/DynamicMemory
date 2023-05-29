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
        [SerializeField] private Color defaultColor;
        [SerializeField] private List<StateColor> stateColors;

        private ITask task;

        public IObservable<Unit> OnClick => button.OnClickAsObservable();

        public void SetData(ITask task)
        {
            this.task = task;

            title.text = task.Label;

            memoryUsage.text = task.Size.ToMemoryString();

            progress.maxValue = task.MaxLifetime;
            task.Lifetime.Subscribe(value => progress.value = value);

            task.Status.Subscribe(OnStatusChanged);
            OnStatusChanged(task.Status.Value);
        }

        public void SetWidth(float width)
        {
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
        }

        private void OnStatusChanged(Task.State state)
        {
            foreach (var item in stateColors)
            {
                if (item.state != state) continue;

                background.color = item.color;
                return;
            }

            background.color = defaultColor;
        }


        [System.Serializable]
        public class StateColor
        {
            public Task.State state;
            public Color color;
        }
    }
}
