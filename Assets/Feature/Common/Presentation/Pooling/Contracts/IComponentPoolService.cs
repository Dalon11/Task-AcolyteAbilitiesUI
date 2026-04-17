using System;
using UnityEngine;

namespace Feature.Common.Presentation.Pooling.Contracts
{

    public interface IComponentPoolService : IDisposable
    {
        TComponent Acquire<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component;

        void Release(Component instance);
    }
}
