namespace StingyJunk.Extensions
{
    using System.Collections.Generic;

    public static class Lists
    {
        /// <summary>
        ///     Cheap circular linked list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> currentNode)
        {
            return currentNode.Next ?? currentNode.List.First;
        }

        /// <summary>
        ///     Cheap circular linked list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> currentNode)
        {
            return currentNode.Previous ?? currentNode.List.Last;
        }
    }
}