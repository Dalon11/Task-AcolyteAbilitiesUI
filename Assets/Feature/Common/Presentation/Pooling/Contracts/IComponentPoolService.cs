using System;
using UnityEngine;

namespace Feature.Common.Presentation.Pooling.Contracts
{
    /// <summary>
    /// Контракт общего пула Component для переиспользования UI-элементов между фичами.
    /// </summary>
    public interface IComponentPoolService : IDisposable
    {
        TComponent Acquire<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component;

        void Release(Component instance);
    }
}
