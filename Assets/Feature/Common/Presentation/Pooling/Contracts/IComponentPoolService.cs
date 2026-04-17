using System;
using UnityEngine;

namespace Feature.Common.Presentation.Pooling.Contracts
{

    public interface IComponentPoolService : IDisposable
    {
        public TComponent Acquire<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component;

        public void Release(Component instance);
    }
}
