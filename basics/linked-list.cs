// See https://aka.ms/new-console-template for more information
using System;

class Program
{
    //generic Node class using <T>
    public class Node<T>
    {
        public T data;
        public Node<T>? Next;

        //Node constructor
        public Node(T data)
        {
            this.data = data;
            this.Next = null;
        }
    }

    //generic LinkedList class can be of any type Node<T>
    public class LinkedList<T>
    {
        //'?' means the variable is allowed to be null
        private Node<T>? head;

        public void AppendLast(T data)
        {
            //creates a new Node<T> internally
            var new_node = new Node<T>(data);

            //head is null (list is empty)
            if (head == null)
            {
                head = new_node;
                return;
            }

            //head isn't null, (list has nodes already)
            //traverse list to last node, set new node to be last
            var current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = new_node;
        }

        public void PrintList()
        {
            var current = head;
            while (current != null)
            {
                Console.Write(current.data + "->");
                current = current.Next;
            }
            //last node is null
            Console.WriteLine("null");
        }

        public void MergeSort(LinkedList<T> list)
        {

        }

    }

    static void Main()
    {
        var ll = new LinkedList<int>(); //linkedlist object of type 'int'

        //unsorted array
        int[] arr = {45, 12, 78, 3, 56, 89, 23, 67, 34, 11};

        Console.Write("Original: ");
        for(int i=0; i<arr.Length;i++)
        {
            ll.AppendLast(arr[i]);
        }

        ll.PrintList();

    }
}



