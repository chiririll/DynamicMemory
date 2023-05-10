using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public static class InputFieldExtensions
    {
        private static readonly Color color = new Color(1, .5f, .5f);

        public static void HighlightUntilClick(this TMP_InputField inputField)
        {
            var disp = new CompositeDisposable();
            var image = inputField.image;
            image.color = color;

            inputField.onSelect.AsObservable().Subscribe(_ => ReturnNormalColor(disp, image, inputField.colors.normalColor)).AddTo(disp);
        }

        private static void ReturnNormalColor(
            CompositeDisposable disp,
            Image image,
            Color color)
        {
            disp.Dispose();
            image.color = color;
        }
    }
}
