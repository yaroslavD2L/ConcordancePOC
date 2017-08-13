using System.Collections.Concurrent;

namespace Concordance.Data.Default
{
	internal sealed class Queue<T> : IQueue<T>
	{
		private ConcurrentQueue<T> m_internalQueue = new ConcurrentQueue<T>();

		public bool TryDequeue(out T result)
		{
			return m_internalQueue.TryDequeue(out result);
		}

		public void Enqueue(T item)
		{
			m_internalQueue.Enqueue(item);
		}
	}
}
