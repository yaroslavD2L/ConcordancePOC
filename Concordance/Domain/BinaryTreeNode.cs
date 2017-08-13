namespace Concordance.Domain
{
	internal sealed class BinaryTreeNode<T>
	{
		public T Value { get; private set; }
		public BinaryTreeNode<T> Left { get; set; }
		public BinaryTreeNode<T> Right { get; set; }

		public BinaryTreeNode(
			T data
		)
		{
			Value = data;
		}
	}
}