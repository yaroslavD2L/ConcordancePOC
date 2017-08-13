namespace Concordance.Data
{
	internal interface IQueue<T>
	{
		bool TryDequeue(out T result);

		void Enqueue(T item);
	}
}