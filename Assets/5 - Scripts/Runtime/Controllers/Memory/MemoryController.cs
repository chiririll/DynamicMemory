using DynamicMem.Model;
using System;
using UniRx;
using UnityEngine;

namespace DynamicMem
{
    public class MemoryController : MonoBehaviour, IDisposable
    {
        private Memory memory;
        private CompositeDisposable disp = new();

        [SerializeField] private TaskItem taskPrefab;

        public void SetData(Memory memory)
        {
            disp.Clear();
            // TODO: Clear prefabs

            this.memory = memory;

            memory.OnTaskEnqueue.Subscribe(CreateTaskInQueue).AddTo(disp);
            memory.OnTaskLoaded.Subscribe(LoadTask).AddTo(disp);
            memory.OnTaskMoved.Subscribe(MoveTask).AddTo(disp);
            memory.OnTaskUnloaded.Subscribe(UnloadTask).AddTo(disp);

            // TODO: Load current state
        }

        private void CreateTaskInQueue(ITask task)
        {

        }

        private void LoadTask(ITask task)
        {

        }

        private void MoveTask(ITask task)
        {

        }

        private void UnloadTask(ITask task)
        {

        }

        public void Dispose()
        {
            disp.Dispose();
        }
    }
}
