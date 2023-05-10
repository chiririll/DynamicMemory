using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class AlertController : MonoBehaviour
    {
        [SerializeField] private GameObject alertRoot;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private Action onConfirm;
        private Action onCancel;

        private void Awake()
        {
            confirmButton.OnClickAsObservable().Subscribe(_ => Hide(true)).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ => Hide(false)).AddTo(this);
            
            alertRoot.SetActive(false);
            DI.Add(this);
        }

        public void Show(Action onConfirm = null, Action onCancel = null)
        {
            this.onConfirm = onConfirm;
            this.onCancel = onCancel;

            alertRoot.SetActive(true);
        }

        private void Hide(bool success = false)
        {
            if (success)
            {
                onConfirm?.Invoke();
            }
            else
            {
                onCancel?.Invoke();
            }

            onConfirm = null;
            onCancel = null;

            alertRoot.SetActive(false);
        }
    }
}
