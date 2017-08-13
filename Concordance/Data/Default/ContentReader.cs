using System.Linq;
using System.Collections.Generic;
using Concordance.Domain;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Concordance.Services;

namespace Concordance.Data.Default
{
	/// <summary>
	/// Can be implemented much smarter. The sentence can consist of nested sentences and so on and so on. 
	/// Parsing can be done by separate algorithms.This POC is not about that.
	/// </summary>
	internal sealed class ContentReader : IInputReader<Word>
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

		public IEnumerable<IEnumerable<Word>> Read()
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

					if (!IsEndOfSentence(character) && !streamReader.EndOfStream)
					{
						continue;
					}

					Sentence currentSentence = new Sentence(sentenceId++);

					currentSentence.Match(
						buffer.ToString()
					);

					yield return currentSentence;

					buffer.Clear();
				}
			}
		}

		private bool IsEndOfSentence(char character)
		{
			return SentenceEndCharacters.Contains(character);
		}
	}
}