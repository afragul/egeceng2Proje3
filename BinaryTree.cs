using System.Text;

namespace Name2
{
    public class TreeNode <T> where T : IComparable<T>
    {
        public T Value;
        public TreeNode<T> sol;
        public TreeNode<T> sag;
        public TreeNode(T deger)
        {
            Value = deger;
            sol = null;
            sag = null;
        }
    }
    public class BinaryTree<T> where T : IComparable<T>
    {      
        public TreeNode<T> Root { get; private set; }
    
        public void Insert(T value) //eklemek icin
        {
            Root = InsertRecursively(Root, value);
        }
    
        private TreeNode<T> InsertRecursively(TreeNode<T> node, T value) 
        {
            if (node == null)
                return new TreeNode<T>(value);
    
            if (value.CompareTo(node.Value) < 0)
                node.sol = InsertRecursively(node.sol, value);
            else if (value.CompareTo(node.Value) > 0)
                node.sag = InsertRecursively(node.sag, value);
    
            return node;
        }
        public string InOrderTraversal() //inorder yazdirmak icin

        {
            var result = new StringBuilder();
            InOrderTraversal(Root, result);
            return result.ToString().TrimEnd();
        }

        private void InOrderTraversal(TreeNode<T> node,StringBuilder result)
        {
            if (node == null) return;
            InOrderTraversal(node.sol,result);
            Console.Write($"{node.Value} ");
            InOrderTraversal(node.sag,result);
        }
        public bool Search(T value) //agacta arama yapmak icin 
        {
            return SearchRecursively(Root, value);
        }

        private bool SearchRecursively(TreeNode<T> node, T value)
        {
            if (node == null)
                return false;

            if (value.CompareTo(node.Value) == 0)
                return true;
            else if (value.CompareTo(node.Value) < 0)
                return SearchRecursively(node.sol, value);
            else
                return SearchRecursively(node.sag, value);
        }
    }  
}