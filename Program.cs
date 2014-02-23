using System;

namespace AVLTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = new AVLTree<int>();
            for (int i = 0; i < 41; i++)
                t.Add(i);
            for (int i = 0; i < 36; i++)
                t.Remove(i);
            Console.ReadLine();
        }
    }

    internal class AVLTree<T> where T : IComparable<T>
    {
        private Node<T> _root;

        public int Count { get; private set; }

        public void Add(T item)
        {
            Count++;
            _root = Add(item, _root);
        }

        private static Node<T> Add(T item, Node<T> at)
        {
            if (at == null)
                return new Node<T> {Value = item};
            if (item.CompareTo(at.Value) <= 0)
                at.Left = Add(item, at.Left);
            else
                at.Right = Add(item, at.Right);
            return Node<T>.Balance(at);
        }

        public void Remove(T value)
        {
            _root = Remove(value, _root);
        }

        private static Node<T> RemoveMin(Node<T> at, ref T minValue)
        {
            if (at.Left == null)
            {
                minValue = at.Value;
                return at.Right;
            }
            at.Left = RemoveMin(at.Left, ref minValue);
            return Node<T>.Balance(at);
        }

        private Node<T> Remove(T value, Node<T> at)
        {
            if (at == null) return null;
            int comparison = value.CompareTo(at.Value);
            if (comparison < 0)
                at.Left = Remove(value, at.Left);
            else if (comparison > 0)
                at.Right = Remove(value, at.Right);
            else //comparison == 0
            {
                if (at.Left == null)
                    return at.Right;
                if (at.Right == null)
                    return at.Left;
                //both children exist
                RemoveMin(at.Right, ref at.Value);
            }
            return Node<T>.Balance(at);
        }

        private class Node<T>
        {
            public Node<T> Left, Right;
            public T Value;
            private int _height = 1;

            private void FixHeight()
            {
                _height = Math.Max(HeightOf(Left), HeightOf(Right)) + 1;
            }

            private static int HeightOf(Node<T> n)
            {
                return n != null ? n._height : 0;
            }

            private int BalanceFactor()
            {
                return HeightOf(Left) - HeightOf(Right);
            }

            private static Node<T> RotateRight(Node<T> n)
            {
                Node<T> result = n.Left;
                n.Left = result.Right;
                result.Right = n;
                n.FixHeight();
                result.FixHeight();
                return result;
            }

            private static Node<T> RotateLeft(Node<T> n)
            {
                Node<T> result = n.Right;
                n.Right = result.Left;
                result.Left = n;
                n.FixHeight();
                result.FixHeight();
                return result;
            }

            public static Node<T> Balance(Node<T> n)
            {
                n.FixHeight();
                if (n.BalanceFactor() == -2)
                {
                    if (n.Right.BalanceFactor() > 0)
                        n.Right = RotateRight(n.Right);
                    return RotateLeft(n);
                }
                if (n.BalanceFactor() == 2)
                {
                    if (n.Left.BalanceFactor() < 0)
                        n.Left = RotateLeft(n.Left);
                    return RotateRight(n);
                }
                return n;
            }
        }
    }
}