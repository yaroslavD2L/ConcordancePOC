using System;
using System.Collections.Generic;

namespace Concordance.Domain
{
	internal sealed class BinaryTreeNode<T> : IComparable<BinaryTreeNode<T>>
		where T : IReduceable<T>
	{
		private readonly IComparer<T> m_comparer;

		public BinaryTreeNode(
			T data,
			IComparer<T> comparer
		)
		{
			Value = data;
			m_comparer = comparer;
		}

		public T Value { get; private set; }
		public BinaryTreeNode<T> Left { get; set; }
		public BinaryTreeNode<T> Right { get; set; }

		public int CompareTo(BinaryTreeNode<T> other)
		{
			return m_comparer.Compare(Value, other.Value);
		}

		public void Merge(BinaryTreeNode<T> right)
		{
			this.Value.Merge(right.Value);
		}
	}
}