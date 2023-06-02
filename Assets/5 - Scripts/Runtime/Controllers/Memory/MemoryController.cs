﻿using DG.Tweening;
using DynamicMem.Config;
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

        private AppConfig config;
        private MemoryManager memory;
        private CompositeDisposable disp = new();

        private Dictionary<TaskId, TaskItem> tasks = new();

        public void SetData(MemoryManager memory)
        {
            Dispose();

            this.memory = memory;
            this.config = DI.Get<AppConfig>();

            memory.OnTaskEnqueue.Subscribe(CreateTaskInQueue).AddTo(disp);
            memory.OnTaskLoaded.Subscribe(LoadTask).AddTo(disp);
            memory.OnTaskMoved.Subscribe(MoveTask).AddTo(disp);
            memory.OnTaskUnloaded.Subscribe(UnloadTask).AddTo(disp);

            memory.OnCleanupRequested.Subscribe(_ => Cleanup()).AddTo(disp);

            // TODO: Load current state
        }

        private void CreateTaskInQueue(ITask task)
        {
            var taskItem = Instantiate(taskPrefab, queueContainer);
            tasks.Add(task.Id, taskItem);
            taskItem.OnClick.Subscribe(_ => memory.SelectTask(task)).AddTo(taskItem);
            
            SetTaskWidth(task, taskItem);

            taskItem.SetData(task);
        }

        private void LoadTask(ITask task)
        {
            var item = tasks[task.Id];

            item.transform.SetParent(memoryContainer, true);

            SetTaskPosition(task, item);
        }

        private void MoveTask(ITask task)
        {
            var item = tasks[task.Id];

            SetTaskPosition(task, item);
        }

        private void UnloadTask(ITask task)
        {
            var item = tasks[task.Id];

            tasks.Remove(task.Id);
            item.transform.DOMoveY(-100, config.simulation.TickTime).SetEase(Ease.Linear);
            Destroy(item.gameObject, config.simulation.TickTime);
        }

        private void SetTaskPosition(ITask task, TaskItem item)
        {
            var k = memoryContainer.rect.width / config.memory.Size;
            var duration = config.simulation.TickTime / 2;
            item.SetWidth(k * task.Size);
            item.transform.DOMoveX(
                memoryContainer.position.x + k * task.Address,
                duration);
            item.transform.DOMoveY(memoryContainer.position.y, duration);
        }

        private void SetTaskWidth(ITask task, TaskItem item)
        {
            var k = memoryContainer.rect.width / config.memory.Size;
            item.SetWidth(k * task.Size);
        }

        private void Cleanup()
        {
            foreach (var item in tasks)
            {
                Destroy(item.Value.gameObject);
            }
            tasks.Clear();
        }

        public void Dispose()
        {
            disp.Clear();
            Cleanup();
        }
    }
}
