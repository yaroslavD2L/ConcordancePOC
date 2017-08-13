using Concordance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Concordance
{
	internal sealed class ContentWriter
	{
		private string IndexToString(Word word)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var item in word.Index)
			{
				sb.Append(item);
				sb.Append(",");
			}

			if (sb.Length > 0)
			{
				sb.Remove(sb.Length - 1, 1);
			}
			else
			{
				return "0";
			}

			return sb.ToString();
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
