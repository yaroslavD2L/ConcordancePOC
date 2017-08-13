using System;
using System.Collections.Generic;

namespace Concordance.Services
{
	public interface IInputReader<T>
	{
		IEnumerable<IEnumerable<T>> Read();
	}
}