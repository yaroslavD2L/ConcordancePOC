using Concordance.Domain;
using System.Collections.Generic;

namespace Concordance.Services
{
	internal interface IMapReduceService<TItem, TResult>
		where TItem : IReduceable<TItem>
	{
		TResult Map(IEnumerable<TItem> chunk);

		TResult Reduce(TResult tree, TResult otherTree);
	}
}