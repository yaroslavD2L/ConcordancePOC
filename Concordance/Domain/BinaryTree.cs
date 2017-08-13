using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Concordance.Domain
{
	internal sealed class BinaryTree<T>
		: IEnumerable<T>
		where T : IReduceable<T>
	{
		public BinaryTree(BinaryTreeNode<T> root = null)
		{
			Root = root;
		}

		public BinaryTreeNode<T> Root { get; private set; }

		public void Add(BinaryTreeNode<T> newNode)
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
				comparisonResult = current.CompareTo(newNode);
				if (comparisonResult == 0)
				{
					current.Merge(newNode);
					return;
				}

				parent = current;
				current = comparisonResult > 0 // if current is greate that new Node
					? current.Left
					: current.Right;
			}

			comparisonResult = parent.CompareTo(newNode);

			if (comparisonResult > 0)
			{
				parent.Left = newNode;
			}
			else
			{
				parent.Right = newNode;
			}
		}

		public void Merge(BinaryTree<T> other)
		{
			if (other == null)
			{
				return;
			}

			BinaryTreeNode<T> leftList = ToList(Root, null);
			BinaryTreeNode<T> rightList = ToList(other.Root, null);

			int size;
			BinaryTreeNode<T> list = MergeLists(leftList, rightList, out size);
			Root = ToBinarySearchTreeRoot(ref list, size);
		}

		private BinaryTreeNode<T> MergeLists(BinaryTreeNode<T> left, BinaryTreeNode<T> right, out int size)
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
					int comparisonResult = left.CompareTo(right);

					if (comparisonResult == 0)
					{
						left.Merge(right);
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

		private BinaryTreeNode<T> ToList(BinaryTreeNode<T> tree, BinaryTreeNode<T> head)
		{
			if (tree == null)
			{
				return head;
			}

			head = ToList(tree.Left, head);
			tree.Left = head;
			BinaryTreeNode<T> result = ToList(tree.Right, tree);
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
