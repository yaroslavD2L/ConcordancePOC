using Concordance.Data;
using Concordance.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Concordance.Services.Default
{
	internal sealed class MapWorker<TItem, TResult> : IWorker
		where TItem : IReduceable<TItem>
	{
		private readonly IQueue<TResult> m_queue;
		private readonly IMapReduceService<TItem, TResult> m_mapReduceService;
		private readonly IInputReader<TItem> m_inputReader;

		public MapWorker(
			IQueue<TResult> queue,
			IMapReduceService<TItem, TResult> mapReduceService,
			IInputReader<TItem> inputReader
		)
		{
			m_queue = queue;
			m_mapReduceService = mapReduceService;
			m_inputReader = inputReader;
		}

		public async Task Execute()
		{
			List<Task> mappingTasks = new List<Task>();
			foreach (IEnumerable<TItem> chunk in m_inputReader.Read())
			{
				Task mapping = Map(chunk);

				mappingTasks.Add(mapping);
			}

			await Task.Factory.ContinueWhenAll(mappingTasks.ToArray(), (x) =>
			{
				Console.WriteLine("All Map jobs are done.");
			});
		}

		private Task Map(IEnumerable<TItem> sentence)
		{
			return Task.Factory.StartNew(() =>
			{
				return m_mapReduceService.Map(sentence);
			})
			.ContinueWith((Task<TResult> task) =>
			{
				m_queue.Enqueue(task.Result);
			});
		}
	}
}
