using Problem01.List;
using Problem02.Stack;
using System;
using System.Linq;

namespace LiveDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stack = new Stack<int>();
            stack.Push(5);
            stack.Push(6);
            stack.Push(1);

            Console.WriteLine(String.Join(", ", stack.ToList()));
            Console.WriteLine(stack.Peek());
        }
    }
}
