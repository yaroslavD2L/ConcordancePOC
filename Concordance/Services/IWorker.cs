using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concordance.Services
{
	public interface IWorker
	{
		Task Execute();
	}
}
