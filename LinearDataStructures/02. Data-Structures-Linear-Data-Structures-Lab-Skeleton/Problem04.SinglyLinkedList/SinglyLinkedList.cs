namespace Problem04.SinglyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SinglyLinkedList<T> : IAbstractLinkedList<T>
    {
        private class Node
        {
            public T Element { get; set; }

            public Node Next { get; set; }

            public Node(T element, Node next)
            {
                this.Element = element;
                this.Next = next;
            }

            public Node(T element)
            {
                this.Element = element;
            }
        }

        private Node head;

        public int Count { get; private set; }

        public void AddFirst(T item)
        {
            var node = new Node(item, this.head);

            this.head = node;

            this.Count++;
        }

        public void AddLast(T item)
        {
            var newNode = new Node(item);

            if (this.head == null)
            {
                this.head = newNode;
            }
            else
            {
                var node = this.head;

                while (node.Next != null)
                {
                    node = node.Next;
                }

                node.Next = newNode;
            }

            this.Count++;
        }

        public T GetFirst()
        {
            if (this.head == null)
            {
                throw new InvalidOperationException("Empty Collection");
            }

            return this.head.Element;
        }

        public T GetLast()
        {
            if (this.head == null)
            {
                throw new InvalidOperationException("Empty Collection");
            }

            var node = this.head;

            while (node.Next != null)
            {
                node = node.Next;
            }

            return node.Element;
        }

        public T RemoveFirst()
        {
            if (this.head == null)
            {
                throw new InvalidOperationException("Empty Collection");
            }

            var oldHead = this.head;
            this.head = oldHead.Next;

            this.Count--;

            return oldHead.Element;
        }

        public T RemoveLast()
        {
            var node = this.head;

            if (node == null)
            {
                throw new InvalidOperationException("Empty Collection");
            }
            else if (node.Next == null)
            {
                this.head = null;
                this.Count--;
                return node.Element;
            }
            else
            {
                while (node.Next.Next != null)
                {
                    node = node.Next;
                }

                node.Next = null;
                this.Count--;
                return node.Element;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = this.head;

            while (node != null)
            {
                yield return node.Element;
                node = node.Next;
            }
        }
         
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}