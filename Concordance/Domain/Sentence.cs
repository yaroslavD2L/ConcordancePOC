using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Concordance.Domain
{
	internal sealed class Sentence : Collection<Word>
	{
		private readonly static Regex wordPattern = new Regex(@"[^\W\d](\w|[-']{1,2}(?=\w))*");
		private readonly int m_sentenceId;

		public Sentence(int sentenceId)
		{
			m_sentenceId = sentenceId;
		}

		internal void Match(string line)
		{
			line.ToLowerInvariant();

			foreach (Match match in wordPattern.Matches(line))
			{
				Add(new Word(match.Groups[0].Value, m_sentenceId));
			}
		}
	}
}
