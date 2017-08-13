using System;
using System.Collections.Generic;
using System.Text;

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
