using System.Collections.Generic;

namespace ConcurrentHashSetQueue {
    public class ConcurrentHashSetQueue<T> : IConcurrentQueue<T> {
        private readonly HashSet<T> _queueHashSet = new HashSet<T>();
        private readonly Queue<T> _queue = new Queue<T>();

        private readonly object _lock = new object();

        public void Enqueue(T item) {
            lock (_lock) {
                _queueHashSet.Add(item);
                _queue.Enqueue(item);
            }
        }

        public bool Contains(T item) {
            lock (_lock) {
                return _queueHashSet.Contains(item);
            }
        }

        public int Count() {
            lock (_lock) {
                return _queueHashSet.Count;
            }
        }

        public T Dequeue() {
            lock (_lock) {
                var item = _queue.Dequeue();
                _queueHashSet.Remove(item);
                return item;
            }
        }
    }
}