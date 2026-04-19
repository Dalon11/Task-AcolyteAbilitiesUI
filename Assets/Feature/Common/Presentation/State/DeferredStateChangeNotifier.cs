using System;

namespace Feature.Common.Presentation.State
{

    /// <summary>
    /// Отложенно уведомляет об изменении состояния во время пакетных обновлений.
    /// </summary>
    public sealed class DeferredStateChangeNotifier
    {
        private int _batchDepth;
        private bool _hasPendingStateChange;

        public void BeginBatch()
        {
            _batchDepth++;
        }

        public void EndBatch(Action onStateChanged)
        {
            if (_batchDepth <= 0)
                throw new InvalidOperationException("Завершение батча изменений состояния вызвано без соответствующего Begin.");

            _batchDepth--;
            if (_batchDepth != 0 || !_hasPendingStateChange)
                return;

            _hasPendingStateChange = false;
            Invoke(onStateChanged);
        }

        public void Notify(Action onStateChanged)
        {
            if (_batchDepth > 0)
            {
                _hasPendingStateChange = true;
                return;
            }

            Invoke(onStateChanged);
        }

        private void Invoke(Action handler)
        {
            if (handler != null)
                handler.Invoke();
        }
    }
}
