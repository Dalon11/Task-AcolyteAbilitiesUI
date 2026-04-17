using System;
using UnityEngine;

namespace Feature.Common.Presentation.Pooling.Contracts
{
    /// <summary>
    /// �������� ������ ���� Component ��� ����������������� UI-��������� ����� ������.
    /// </summary>
    public interface IComponentPoolService : IDisposable
    {
        TComponent Acquire<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component;

        void Release(Component instance);
    }
}


