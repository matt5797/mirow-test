using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Helper
{
    public class Tree<T>
    {
        public T Node { get; set; }
        public List<Tree<T>> Children { get; set; } = new List<Tree<T>>();

        public void printTree()
        {
            Debug.Log(Node);
            foreach (Tree<T> child in Children)
                printTree(child);
        }
        public void printTree(Tree<T> root)
        {
            Debug.Log(root.Node);
            foreach (Tree<T> child in root.Children)
                printTree(child);
        }
    }
}