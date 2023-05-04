namespace DynamicMem
{
    public class LazyInject<T>
    {
        private T value;

        public T Value => value ??= DI.Get<T>();

        public static implicit operator T(LazyInject<T> lazyInject) => lazyInject.Value;
    }
}
