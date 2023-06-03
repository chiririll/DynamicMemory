using DG.Tweening;
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

        private readonly LazyInject<AnimationManager> animManager = new();

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
            foreach (var task in memory.LoadedTasks)
            {
                CreateTaskInQueue(task);
                LoadTask(task);
            }
            foreach (var task in memory.Queue)
            {
                CreateTaskInQueue(task);
            }
        }

        private void CreateTaskInQueue(ITask task)
        {
            var taskItem = Instantiate(taskPrefab, queueContainer);
            taskItem.SetData(task, animManager.Value);
            
            tasks.Add(task.Id, taskItem);
            taskItem.OnClick.Subscribe(_ => memory.SelectTask(task)).AddTo(taskItem);
            
            SetTaskWidth(task, taskItem);
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

            if (animManager.Value.Enabled == false)
            {
                SetTaskPosition(task, item);
                return;
            }

            var k = memoryContainer.rect.width / config.memory.Size;
            item.transform.DOMoveX(
                        memoryContainer.position.x + k * task.Address,
                        animManager.Value.TaskSlideXTime);
        }

        private void UnloadTask(ITask task)
        {
            var item = tasks[task.Id];

            tasks.Remove(task.Id);
            if (animManager.Value.Enabled)
            {
                item.transform.DOMoveY(-100, animManager.Value.UnloadTaskTime).SetEase(Ease.Linear);
                Destroy(item.gameObject, animManager.Value.UnloadTaskTime);
            }
            else
            {
                Destroy(item.gameObject);
            }
        }

        private void SetTaskPosition(ITask task, TaskItem item)
        {
            var k = memoryContainer.rect.width / config.memory.Size;
            //item.SetWidth(k * task.Size);

            if (animManager.Value.Enabled)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(
                    item.transform.DOMoveY(
                        item.transform.position.y + (memoryContainer.position.y - item.transform.position.y) / 2f, 
                        animManager.Value.TaskSlideYTime));
                sequence.Append(
                    item.transform.DOMoveX(
                        memoryContainer.position.x + k * task.Address,
                        animManager.Value.TaskSlideXTime));
                sequence.Append(
                    item.transform.DOMoveY(
                        memoryContainer.position.y, 
                        animManager.Value.TaskSlideYTime));
            }
            else
            {
                item.transform.position = new Vector3(
                    memoryContainer.position.x + k * task.Address, 
                    memoryContainer.position.y, item.transform.position.z);
            }
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
