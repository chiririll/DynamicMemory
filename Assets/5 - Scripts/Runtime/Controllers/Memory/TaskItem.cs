using DynamicMem.Model;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class TaskItem : MonoBehaviour
    {
        private static readonly string[] MemoryUnits = { "B", "KB", "MB", "GB", "TB", "PB" };
        private const int MemoryMax = 1 << 10;

        [SerializeField] private RectTransform rt;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text memoryUsage;
        [SerializeField] private Slider progress;
        [SerializeField] private Image background;
        [Space]
        [SerializeField] private Color defaultColor;
        [SerializeField] private List<StateColor> stateColors;

        private ITask task;

        public void SetData(ITask task)
        {
            this.task = task;

            title.text = task.Label;

            memoryUsage.text = GetMemoryString(task.Size);

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

        private string GetMemoryString(int memory)
        {
            if (memory < 0) 
                memory = 0;

            int currentUnit = 0;
            while (memory >= MemoryMax)
            {
                currentUnit++;
                memory /= MemoryMax;
            }

            return memory.ToString() + " " + MemoryUnits[currentUnit];
        }

        [System.Serializable]
        public class StateColor
        {
            public Task.State state;
            public Color color;
        }
    }
}
