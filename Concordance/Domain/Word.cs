﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Concordance.Domain
{
	internal sealed class Word : IMergeable<Word>
	{
		public Word(string value, int index)
		{
			Value = value;
			Index = new HashSet<int>(new[] { index });
			Frequency = 1;
		}

		public string Value { get; private set; }

		public HashSet<int> Index { get; set; }

		public int Frequency { get; set; }

		public void Merge(Word other)
		{
			Frequency += other.Frequency;
			Index.UnionWith(other.Index);
		}
	}
}
