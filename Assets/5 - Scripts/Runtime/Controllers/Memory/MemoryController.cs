using DynamicMem.Model;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace DynamicMem
{
    public class MemoryController : MonoBehaviour, IDisposable
    {
        [SerializeField] private TaskItem taskPrefab;
        [Space]
        [SerializeField] private Transform queueContainer;
        [SerializeField] private RectTransform memoryContainer;

        private MemoryManager memory;
        private CompositeDisposable disp = new();

        private Dictionary<TaskId, TaskItem> tasks = new();

        public void SetData(MemoryManager memory)
        {
            disp.Clear();
            
            foreach (var item in tasks)
            {
                if (item.Value != null)
                {
                    Destroy(item.Value.gameObject);
                }
            }

            this.memory = memory;

            memory.OnTaskEnqueue.Subscribe(CreateTaskInQueue).AddTo(disp);
            memory.OnTaskLoaded.Subscribe(LoadTask).AddTo(disp);
            memory.OnTaskMoved.Subscribe(MoveTask).AddTo(disp);
            memory.OnTaskUnloaded.Subscribe(UnloadTask).AddTo(disp);

            // TODO: Load current state
        }

        private void CreateTaskInQueue(ITask task)
        {
            var taskItem = Instantiate(taskPrefab, queueContainer);
            tasks.Add(task.Id, taskItem);

            taskItem.SetData(task);
        }

        private void LoadTask(ITask task)
        {
            this.LogMsg("Loading task to memory");
        }

        private void MoveTask(ITask task)
        {
            this.LogMsg("Moving task");
        }

        private void UnloadTask(ITask task)
        {
            this.LogMsg("Unloading task");
        }

        public void Dispose()
        {
            disp.Dispose();
        }
    }
}
