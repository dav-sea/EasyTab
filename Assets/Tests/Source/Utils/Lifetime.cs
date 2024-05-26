using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace EasyTab.Tests
{
    internal class Lifetime : IDisposable
    {
        public event Action OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        public T Add<T>(T o)
            where T : Object
        {
            OnDispose += () => Object.DestroyImmediate(o);
            return o;
        }

        public void Add(Action onDispose)
        {
            OnDispose += onDispose;
        }

        public static IEnumerator DefineAsync(Func<Lifetime, UniTask> action)
        {
            return UniTask.ToCoroutine(async () =>
            {
                using (var lifetime = new Lifetime())
                    await action(lifetime);
            });
        }

        public static void Define(Action<Lifetime> action)
        {
            using (var context = new Lifetime())
                action(context);
        }
    }
}