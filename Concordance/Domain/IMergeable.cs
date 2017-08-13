namespace Concordance.Domain
{
	internal interface IMergeable<T>
	{
		void Merge(T other);
	}
}