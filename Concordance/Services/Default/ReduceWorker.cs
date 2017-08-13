using Concordance.Data;
using Concordance.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concordance.Services.Default
{
	internal sealed class ReduceWorker<TItem, TResult> : IWorker
		where TItem : IReduceable<TItem>
	{
		private readonly IQueue<TResult> m_queue;
		private readonly IMapReduceService<TItem, TResult> m_mapReduceService;

		public ReduceWorker(
			IQueue<TResult> queue,
			IMapReduceService<TItem, TResult> mapReduceService
		)
		{
			m_queue = queue;
			m_mapReduceService = mapReduceService;
		}

		public async Task Execute()
		{
			int counter = 10;
			List<Task> reduceTasks = new List<Task>();
			TResult previous = default(TResult);

			TResult output;
			while (true)
			{
				m_queue.TryDequeue(out output);

				if (output == null)
				{
					if (counter == 0)
					{
						break;
					}
					Console.WriteLine("Wait....");
					await Task.Delay(5);
					counter--;
					continue;
				}

				if (previous == null)
				{
					previous = output;
					output = default(TResult);
					continue;
				}

				Task reduceTask = Reduce(previous, output);
				reduceTasks.Add(reduceTask);
				output = previous = default(TResult);
				counter = 10;
			}

			await Task.Factory.ContinueWhenAll(reduceTasks.ToArray(), (x) =>
			{
				m_queue.Enqueue(previous);
				Console.WriteLine("All reduce circles are done.");
			});
		}

		private Task Reduce(TResult left, TResult right)
		{
			return Task.Factory.StartNew(() =>
			{
				return m_mapReduceService.Reduce(left, right);
			})
			.ContinueWith((Task<TResult> reduced) =>
			{
				m_queue.Enqueue(reduced.Result);
			});
		}
	}
}
