using System.Collections.Generic;
using System.Linq;
using UnityEngine;

 public class PayLinesTrie
    {
        private readonly TrieNode root;

        public PayLinesTrie()
        {
            this.root = new TrieNode('@',0,null);
        }

        public TrieNode Prefix(string str)
        {
            var current = root;
            var result = current;
            foreach (var ch in str)
            {
                current = current.GetChildNode(ch);
                if (current == null)
                {
                    break;
                }

                result = current;
            }

            return result;
        }

        public bool Search(string str)
        {
            var prefix = Prefix(str);
            return prefix.depth == str.Length && prefix.GetChildNode('#') != null;
        }

        public void Insert(string item)
        {
            var commonPrefix = Prefix(item);
            var current = commonPrefix;
            for (var i = current.depth; i < item.Length; i++)
            {
                var newNode = new TrieNode(item[i],current.depth + 1, current);
                current.children.Add(newNode);
                current = newNode;
            }
            current.children.Add(new TrieNode('#', current.depth + 1,current));
        }
        
        public void InsertRange(List<string> items)
        {
            foreach (var item in items)
            {
                Insert(item);
            }
        }

        public void Remove(string item)
        {
            if (!Search(item))
            {
                Debug.Log("Item Not Found !");
                return;
            }

            var node = Prefix(item).GetChildNode('#');
            while (node.IsLeaf())
            {
                var parent = node.parent;
                parent.RemoveChildNode(node.value);
                node = parent;
            }
        }
        
    }

    public class TrieNode
    {
        public char value;
        public List<TrieNode> children;
        public TrieNode parent;
        public int depth;

        public TrieNode(char value,int depth,TrieNode parent)
        {
            this.value = value;
            this.depth = depth;
            this.parent = parent;
            this.children = new List<TrieNode>();
        }

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public TrieNode GetChildNode(char c)
        {
            return this.children.FirstOrDefault(ch => ch.value == c);
        }

        public void RemoveChildNode(char c)
        {
            var ch = GetChildNode(c);
            if (ch == null)
            {
                Debug.Log("Child Node not found !");
                return;
            }
            this.children.Remove(ch);
        }
    }