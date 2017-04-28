namespace ConcurrentHashSetQueue {
    public interface IConcurrentQueue<T> {
        void Enqueue(T item);

        bool Contains(T item);

        int Count();

        T Dequeue();
    }
}