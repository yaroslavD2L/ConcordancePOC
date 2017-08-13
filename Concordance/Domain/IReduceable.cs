namespace Concordance.Domain
{
	internal interface IReduceable<T>
	{
		void Merge(T other);
	}
}