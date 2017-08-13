using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Concordance.Domain;
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Concordance
{
	/// <summary>
	/// Can be implemented much smarter. The sentence can consist of nested sentences and so on and so on. 
	/// Parsing can be done by separate algorithms.This POC is not about that.
	/// A sentence in this case is a line in the file.
	/// </summary>
	internal sealed class ContentReader
	{
		private readonly string m_filePath;
		private readonly char[] SentenceEndCharacters = new[] { '.', '!', '?' };

		public ContentReader(string filePath)
		{
			m_filePath = filePath;
		}

		private async Task<Sentence> ConvertToSentence(Task<string> readTask, int sentenceId)
		{
			string sentenceText = await readTask;

			Sentence currentSentence = new Sentence(sentenceId);

			currentSentence.Match(
				sentenceText.ToLowerInvariant()
			);

			return currentSentence;
		}

		public IEnumerable<Sentence> Read()
		{
			List<Task> resultList = new List<Task>();
			int sentenceId = 1;
			StringBuilder buffer = new StringBuilder();
			using (StreamReader streamReader = File.OpenText(m_filePath))
			{
				while (!streamReader.EndOfStream)
				{
					char character = (char)streamReader.Read();
					buffer.Append(character);

					if (IsEndOfSentence(character)) {
						Sentence currentSentence = new Sentence(sentenceId++);

						string text = buffer
							.ToString()
							.ToLowerInvariant();

						currentSentence.Match(text);

						buffer.Clear();
						yield return currentSentence;
					}
				}
			}
		}

		private bool IsEndOfSentence(char character)
		{
			return SentenceEndCharacters.Contains(character);
		}
	}
}