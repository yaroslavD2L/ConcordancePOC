using Concordance.Domain;
using System;
using System.Collections.Generic;

namespace Concordance.Data.Default
{
	internal sealed class OutputWriter : IOutputWriter<Word>
	{
		private string IndexToString(Word word)
		{
			return string.Join(",", word.Index);
		}

		public void Write(IEnumerable<Word> words)
		{
			foreach (Word word in words)
			{
				string value = $"{word.Value} {{ {word.Frequency}:{IndexToString(word)} }}";
				Console.WriteLine(value);
			}
		}
	}
}
