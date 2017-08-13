using Concordance.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Concordance
{
	class Program
	{
		private static string file = "./test.txt";
		private static readonly Concordance<Word> Concordance = new Concordance<Word>(
			comparer: new AlfaFrequency()
		);

		private static ContentReader contentReader = new ContentReader(file);
		private static ContentWriter contentWriter = new ContentWriter();


		private static BinaryTree<Word> ResultTree;
		private readonly static ConcurrentQueue<BinaryTree<Word>> ReduceQueue = new ConcurrentQueue<BinaryTree<Word>>();

		static void Main(string[] args)
		{
			MainAsync()
				.GetAwaiter()
				.GetResult();
		}


		static async Task MainAsync()
		{
			List<Task> mappingTasks = new List<Task>();
			foreach (Sentence sentence in contentReader.Read())
			{
				Task mapping = Map(sentence);

				mappingTasks.Add(mapping);
			}

			await Task.Factory.ContinueWhenAll(mappingTasks.ToArray(), (x) =>
			{
				BinaryTree<Word> dequeuedTree;
				if (ReduceQueue.TryDequeue(out dequeuedTree))
				{
					ResultTree = dequeuedTree;
				}
			});

			while (!ReduceQueue.IsEmpty)
			{
				ResultTree = await Reduce(ResultTree);
			}

			contentWriter.Write(ResultTree);

			Console.ReadLine();
		}

		private static Task Map(Sentence sentence)
		{
			return Task.Factory.StartNew(() =>
			{
				return Concordance.Map(sentence);
			})
			.ContinueWith((Task<BinaryTree<Word>> task) =>
			{
				ReduceQueue.Enqueue(task.Result);
			});
		}

		private static Task<BinaryTree<Word>> Reduce(BinaryTree<Word> currentTree)
		{
			BinaryTree<Word> dequeuedTree;
			if (!ReduceQueue.TryDequeue(out dequeuedTree))
			{
				return Task.Factory.StartNew(() => currentTree);
			}

			return Task.Factory.StartNew(() =>
			{
				return Concordance.Reduce(dequeuedTree, currentTree);
			});
		}
	}
}