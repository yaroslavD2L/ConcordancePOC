using Concordance.Domain;
using System.Collections.Generic;

namespace Concordance.Services.Default
{
	internal sealed class MapReduceService<T>
		: IMapReduceService<T, BinaryTree<T>>
		where T : IReduceable<T>
	{
		private readonly IComparer<T> m_comparer;

		public MapReduceService(IComparer<T> comparer)
		{
			m_comparer = comparer;
		}

		public BinaryTree<T> Map(IEnumerable<T> chunk)
		{
			BinaryTree<T> binaryTree = new BinaryTree<T>();

			foreach (T data in chunk)
			{
				BinaryTreeNode<T> nextNode = new BinaryTreeNode<T>(data, m_comparer);

				binaryTree.Add(nextNode);
			}

			return binaryTree;
		}

		public BinaryTree<T> Reduce(BinaryTree<T> tree1, BinaryTree<T> tree2)
		{
			tree1.Merge(tree2);
			return tree1;
		}
	}
}
