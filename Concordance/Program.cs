using Concordance.Data;
using Concordance.Data.Default;
using Concordance.Domain;
using Concordance.Services;
using Concordance.Services.Default;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Concordance
{
	class Program
	{
		private static string file = "./test.txt";
		private static Stopwatch stopWatch = new Stopwatch();

		static void Main(string[] args)
		{
			MainAsync()
				.GetAwaiter()
				.GetResult();
		}

		static async Task MainAsync()
		{
			IInputReader<Word> inputReader = new ContentReader(file);
			IOutputWriter<Word> outputWriter = new OutputWriter();

			IQueue<BinaryTree<Word>> queue = new Data.Default.Queue<BinaryTree<Word>>();
			IMapReduceService<Word, BinaryTree<Word>> mapReducerService = new MapReduceService<Word>(
				comparer: new AlfaFrequency()
			);

			IWorker mapWorker = new MapWorker<Word, BinaryTree<Word>>(
				queue,
				mapReducerService,
				inputReader
			);

			IWorker reduceWorker = new ReduceWorker<Word, BinaryTree<Word>>(
				queue,
				mapReducerService
			);

			stopWatch.Start();

			await Task.Factory.ContinueWhenAll(
				new[] {
					mapWorker.Execute(),
					reduceWorker.Execute(),
				}, (x) =>
				{
					stopWatch.Stop();
					Console.WriteLine($"Output is ready. {stopWatch.ElapsedMilliseconds} ms");
				}
			);

			BinaryTree<Word> dequeuedTree;
			if (queue.TryDequeue(out dequeuedTree))
			{
				outputWriter.Write(dequeuedTree);
			}

			Console.ReadLine();
		}
	}
}