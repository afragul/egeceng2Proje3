using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje3Afragul
{

    class Node
    {
        public int Key { get; private set; }

        public Node(int key)
        {
            Key = key;
        }

        public void SetKey(int newKey)
        {
            Key = newKey;
        }
    }
    class MaxHeap<T> where T : IComparable<T>
    {
        private List<T> heap;

        public MaxHeap()
        {
            heap = new List<T>();
        }

        // Eleman ekleme
        public void Insert(T value)
        {
            heap.Add(value); // Yeni elemanı sona ekle
            HeapifyUp(heap.Count - 1); // Yukarı çıkarma işlemi
        }

        // Maksimum elemanı alma
        public T ExtractMax()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            T max = heap[0]; // Kök eleman
            T last = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1); // Son elemanı çıkar

            if (heap.Count > 0)
            {
                heap[0] = last; // Son elemanı köke taşı
                HeapifyDown(0); // Aşağı indirme işlemi
            }

            return max;
        }

        // Yukarı çıkarma işlemi
        private void HeapifyUp(int index)
        {
            int parentIndex = (index - 1) / 2;

            while (index > 0 && heap[index].CompareTo(heap[parentIndex]) > 0)
            {
                Swap(index, parentIndex);
                index = parentIndex;
                parentIndex = (parentIndex - 1) / 2;
            }
        }

        // Aşağı indirme işlemi
        private void HeapifyDown(int index)
        {
            int largerChild;
            int leftChild;
            int rightChild;
            int size = heap.Count;

            while (index < size / 2) // En az bir çocuk var mı?
            {
                leftChild = 2 * index + 1;
                rightChild = leftChild + 1;

                // Büyük olan çocuğu bul
                if (rightChild < size && heap[leftChild].CompareTo(heap[rightChild]) < 0)
                    largerChild = rightChild;
                else
                    largerChild = leftChild;

                // Eğer kök eleman büyükse, işlemi durdur
                if (heap[index].CompareTo(heap[largerChild]) >= 0)
                    break;

                Swap(index, largerChild);
                index = largerChild;
            }
        }

        
        // Elemanları yer değiştir
        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        // Heap'teki elemanları liste olarak döndür
        public List<T> GetHeapList()
        {
            return heap;
        }

        // Heap'in başındaki (maksimum) elemanı göster
        public T Peek()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            return heap[0];
        }
    }
}
