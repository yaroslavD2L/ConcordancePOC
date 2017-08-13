using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Concordance.Domain
{
	internal sealed class Concordance<T>
		where T : IMergeable<T>
	{
		private readonly IComparer<T> m_comparer;

		public Concordance(IComparer<T> comparer)
		{
			m_comparer = comparer;
		}

		internal BinaryTree<T> Map(IEnumerable<T> content)
		{
			BinaryTree<T> binaryTree = new BinaryTree<T>(m_comparer);

			foreach (T data in content)
			{
				BinaryTreeNode<T> nextNode = new BinaryTreeNode<T>(data);

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
