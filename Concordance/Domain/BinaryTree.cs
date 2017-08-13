using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordance.Domain
{
	internal sealed class BinaryTree<T>
		: IEnumerable<T>
		where T : IMergeable<T>
	{
		private readonly IComparer<T> m_comparer;
		public BinaryTree(IComparer<T> comparer, BinaryTreeNode<T> root = null)
		{
			m_comparer = comparer;
			Root = root;
		}

		public BinaryTreeNode<T> Root { get; private set; }

		internal void Add(BinaryTreeNode<T> newNode)
		{
			if (Root == null)
			{
				Root = newNode;
				return;
			}

			BinaryTreeNode<T> current = Root;
			BinaryTreeNode<T> parent = null;
			int comparisonResult;

			while (current != null)
			{
				comparisonResult = m_comparer.Compare(current.Value, newNode.Value);
				if (comparisonResult == 0)
				{
					current.Value.Merge(newNode.Value);
					return;
				}

				parent = current;
				current = comparisonResult > 0 // if current is greate that new Node
					? current.Left
					: current.Right;
			}

			comparisonResult = m_comparer.Compare(parent.Value, newNode.Value);

			if (comparisonResult > 0)
			{
				parent.Left = newNode;
			}
			else
			{
				parent.Right = newNode;
			}
		}

		public void Merge(BinaryTree<T> right)
		{
			if (right == null)
			{
				return;
			}

			BinaryTreeNode<T> leftList = ToSortedList(Root, null);
			BinaryTreeNode<T> rightList = ToSortedList(right.Root, null);

			int size;
			BinaryTreeNode<T> list = MergeAsSortedLists(leftList, rightList, out size);
			Root = ToBinarySearchTreeRoot(ref list, size);
		}

		private BinaryTreeNode<T> MergeAsSortedLists(BinaryTreeNode<T> left, BinaryTreeNode<T> right, out int size)
		{
			BinaryTreeNode<T> head = null;
			size = 0;

			while (left != null || right != null)
			{
				BinaryTreeNode<T> next;
				if (left == null)
				{
					next = Detach(ref right);
				}
				else if (right == null)
				{
					next = Detach(ref left);
				}
				else
				{
					int comparisonResult = m_comparer.Compare(left.Value, right.Value);

					if (comparisonResult == 0) {
						left.Value.Merge(right.Value);
						Detach(ref right);

						continue;
					}

					next = comparisonResult > 0
						? Detach(ref left)
						: Detach(ref right);
				}

				next.Right = head;
				head = next;
				size++;
			}

			return head;
		}

		private BinaryTreeNode<T> ToBinarySearchTreeRoot(ref BinaryTreeNode<T> head, int size)
		{
			if (size == 0)
			{
				return null;
			}

			BinaryTreeNode<T> root;
			if (size == 1)
			{
				root = head;
				head = head.Right;
				root.Right = null;
				return root;
			}

			int leftSize = size / 2;
			int rightSize = size - leftSize - 1;

			BinaryTreeNode<T> leftRoot = ToBinarySearchTreeRoot(ref head, leftSize);
			root = head;
			head = head.Right;
			root.Left = leftRoot;
			root.Right = ToBinarySearchTreeRoot(ref head, rightSize);
			return root;
		}

		private BinaryTreeNode<T> Detach(ref BinaryTreeNode<T> node)
		{
			var tmp = node;
			node = node.Left;
			tmp.Left = null;
			return tmp;
		}

		private BinaryTreeNode<T> ToSortedList(BinaryTreeNode<T> tree, BinaryTreeNode<T> head)
		{
			if (tree == null)
			{
				return head;
			}

			head = ToSortedList(tree.Left, head);
			tree.Left = head;
			BinaryTreeNode<T> result = ToSortedList(tree.Right, tree);
			tree.Right = null;
			return result;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return GetNext(Root).GetEnumerator();
		}

		private IEnumerable<T> GetNext(BinaryTreeNode<T> current)
		{
			if (current == null)
			{
				return Enumerable.Empty<T>();
			}

			return GetNext(current.Left)
				.Concat(new[] { current.Value })
				.Concat(GetNext(current.Right));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
