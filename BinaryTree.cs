using System;
using System.Text;

namespace Proje3Afragul
{
    public class TreeNode<T> where T : IComparable<T>
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

        public void Insert(T value)
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
        public string InOrderTraversal()
        {
            var result = new StringBuilder();
            InOrderTraversal(Root, result);
            return result.ToString().TrimEnd();
        }

        private void InOrderTraversal(TreeNode<T> node, StringBuilder result)
        {
            if (node == null) return;
            InOrderTraversal(node.sol, result);
            Console.Write($"{node.Value} ");
            InOrderTraversal(node.sag, result);
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

        public (int depht, int count) CalculateDepth() // Agacin derinligini hesaplamak icin
        {
            return CalculateDepthRecursively(Root);
        }

        private (int depht, int count) CalculateDepthRecursively(TreeNode<T> node)
        {
            if (node == null)
                return (0,0);

            var (leftDepth, leftCount) = CalculateDepthRecursively(node.sol);
            var (rightDepth, rightCount) = CalculateDepthRecursively(node.sag);

            int currentDepth = Math.Max(leftDepth, rightDepth);
            int currentNodeCount = 1 + leftCount + rightCount;


            return (currentDepth, currentNodeCount);
        }

        public void IkiHarf(char baslangic, char son)
        {
            IkiHarfInOrder(Root, baslangic, son);
        }

        private void IkiHarfInOrder(TreeNode<T> node, char baslangic, char son)
        {
            if (node == null) return;

            // Sol alt ağacı dolaş
            IkiHarfInOrder(node.sol, baslangic, son);

            // Eğer node.Value EgeDeniziB nesnesiyse, onu doğru tipe dönüştür
            if (node.Value is EgeDeniziB balik)  // node.Value'yi EgeDeniziB türüne dönüştür
            {
                // Balık ismini al
                string balikIsmi = balik.isim;

                // İlk harfi al
                char ilkHarf = balikIsmi[0];  // isim özelliğinden ilk harfi al

                // Geçerli ismin belirtilen harf aralığında olup olmadığını kontrol et
                if (ilkHarf >=  baslangic && ilkHarf <= son)
                {
                    Console.WriteLine(balikIsmi);  // Yalnızca isim yazdır
                }
            }

            // Sağ alt ağacı dolaş
            IkiHarfInOrder(node.sag, baslangic, son);
        }

    }
}