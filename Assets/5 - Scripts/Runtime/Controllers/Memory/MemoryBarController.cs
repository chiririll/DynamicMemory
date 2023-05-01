using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DynamicMem
{
    public class MemoryBarController : MonoBehaviour
    {
        public void Init()
        {

        }

        public async UniTask AddTask(TaskItem task)
        {
            await UniTask.DelayFrame(1);
        }
    }
}
