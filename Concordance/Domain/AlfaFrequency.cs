using System;
using System.Collections.Generic;

namespace Concordance.Domain
{
	internal sealed class AlfaFrequency : IComparer<Word>
	{
		public int Compare(Word x, Word y)
		{
			return StringComparer.OrdinalIgnoreCase.Compare(x.Value, y.Value);
		}
	}
}
