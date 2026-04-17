using System;
using System.Collections.Generic;
using Feature.Common.Presentation.Pooling.Contracts;
using UnityEngine;

namespace Feature.Common.Presentation.Pooling.Services
{

    /// <summary>
    /// Предоставляет сервисную логику Component Pool для сценариев экрана.
    /// </summary>
    public sealed class ComponentPoolService : IComponentPoolService
    {
        private readonly Dictionary<int, Queue<Component>> _instancesByPrefabId = new Dictionary<int, Queue<Component>>();
        private readonly Dictionary<int, int> _prefabIdByInstanceId = new Dictionary<int, int>();
        private readonly HashSet<int> _activeInstanceIds = new HashSet<int>();
        private readonly List<Component> _createdInstances = new List<Component>();

        private bool _isDisposed;

        public TComponent Acquire<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component
        {
            EnsureNotDisposed();

            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));

            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            int prefabId = prefab.GetInstanceID();
            Queue<Component> pool = GetOrCreatePool(prefabId);

            Component pooledInstance = DequeueAliveInstance(pool);
            TComponent instance;

            if (pooledInstance != null)
                instance = (TComponent)pooledInstance;
            else
            {
                instance = UnityEngine.Object.Instantiate(prefab, parent);
                int instanceId = instance.GetInstanceID();
                _prefabIdByInstanceId[instanceId] = prefabId;
                _createdInstances.Add(instance);
            }

            instance.transform.SetParent(parent, false);
            instance.gameObject.SetActive(true);
            _activeInstanceIds.Add(instance.GetInstanceID());

            return instance;
        }

        public void Release(Component instance)
        {
            if (_isDisposed)
                return;

            if (instance == null)
                return;

            int instanceId = instance.GetInstanceID();
            if (!_activeInstanceIds.Contains(instanceId))
                return;

            int prefabId;
            if (!_prefabIdByInstanceId.TryGetValue(instanceId, out prefabId))
                throw new InvalidOperationException("��������� �� ����������� ������������������� ���� �����������.");

            _activeInstanceIds.Remove(instanceId);
            instance.gameObject.SetActive(false);

            Queue<Component> pool = GetOrCreatePool(prefabId);
            pool.Enqueue(instance);
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            for (int i = 0; i < _createdInstances.Count; i++)
            {
                Component component = _createdInstances[i];
                if (component == null)
                    continue;

                UnityEngine.Object.Destroy(component.gameObject);
            }

            _instancesByPrefabId.Clear();
            _prefabIdByInstanceId.Clear();
            _activeInstanceIds.Clear();
            _createdInstances.Clear();
            _isDisposed = true;
        }

        private Queue<Component> GetOrCreatePool(int prefabId)
        {
            Queue<Component> pool;
            if (_instancesByPrefabId.TryGetValue(prefabId, out pool))
                return pool;

            pool = new Queue<Component>();
            _instancesByPrefabId[prefabId] = pool;
            return pool;
        }

        private Component DequeueAliveInstance(Queue<Component> pool)
        {
            while (pool.Count > 0)
            {
                Component component = pool.Dequeue();
                if (component != null)
                    return component;
            }

            return null;
        }

        private void EnsureNotDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ComponentPoolService));
        }
    }
}
