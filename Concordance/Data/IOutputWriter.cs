using System.Collections.Generic;

namespace Concordance.Data
{
	internal interface IOutputWriter<T>
	{
		void Write(IEnumerable<T> output);
	}
}