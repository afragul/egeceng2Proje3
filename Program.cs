using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Proje3Afragul
{
    class EgeDeniziB : IComparable<EgeDeniziB>
    {
        public string isim { get; set; }
        public string bilgi;
        public string[] words;
        public BinaryTree<string> kelimeler;
        public int derinlik;
        private static int toplamDerinlik = 0;
        private int dugum;
        public int dengeliDerinlik;

        public EgeDeniziB(string isim, string info)
        {
            this.isim = isim.ToUpper();
            bilgi = info;
            words = bilgi.Split(new[] { ' ', ',', '.', ';', ':', '!', '?', '-', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            kelimeler = new BinaryTree<string>();
            foreach (var item in words.Where(w => !string.IsNullOrWhiteSpace(w)))
            {
                kelimeler.Insert(item.ToLower());
            }
            derinlik = kelimeler.CalculateDepth().depht -1 ;
            toplamDerinlik += derinlik;
            dugum = kelimeler.CalculateDepth().count;
            dengeliDerinlik = (int)Math.Floor(Math.Log(dugum + 1, 2));
        }
        
        public int CompareTo(EgeDeniziB other)
        {
            if (other == null) return 1;
            return isim.CompareTo(other.isim);
        }
        public override string ToString()
        {
            return ($"{isim} -{kelimeler.InOrderTraversal()} \n");
        }
        public static int GetOrtalamaDerinlik()
        {
            return toplamDerinlik / 38;
            
        }
    }

    class Program
    {
        public static void BubbleSort(int[] array) //bubble sort algoritması (sample)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        // İki öğenin yerlerini değiştir
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
        public static void QuickSort(int[] array) //quick sort algortiması (advance)
        {
            void Sort(int low, int high)
            {
                if (low < high)
                {
                    int pivot = Partition(low, high);
                    Sort(low, pivot - 1);
                    Sort(pivot + 1, high);
                }
            }
            int Partition(int low, int high)
            {
                int pivot = array[high];
                int i = low - 1;

                for (int j = low; j < high; j++)
                {
                    if (array[j] <= pivot)
                    {
                        Swap(++i, j);
                    }
                }
                Swap(i + 1, high);
                return i + 1;
            }

            void Swap(int a, int b)
            {
                (array[a], array[b]) = (array[b], array[a]);
            }

            Sort(0, array.Length - 1);
        }
        public static (BinaryTree<EgeDeniziB> tree, Hashtable hash, MaxHeap<EgeDeniziB> heap) DosyaOku()
        { //dosya okundu ve tree oluşturuldu
            string projeYolu = Directory.GetCurrentDirectory();
            string dosyaAdi = "baliklar.txt";
            string dosyaYolu = Path.Combine(projeYolu, dosyaAdi);
            BinaryTree<EgeDeniziB> balıklar = new BinaryTree<EgeDeniziB>(); //balik treesi oluşturuldu.
            Hashtable baliklarHashTable = new Hashtable(); //Hashtable oluşturuldu.
            MaxHeap<EgeDeniziB> balikHeap = new MaxHeap<EgeDeniziB>(); //Max heap oluşturuldu.
            try
            {
                foreach (var line in File.ReadAllLines(dosyaYolu))
                {
                    var info = line.Split('/');
                    EgeDeniziB balik = new EgeDeniziB(info[0], info[1]);
                    balıklar.Insert(balik); //baliklar tree ye eklendi
                    baliklarHashTable.Add(balik.isim, balik.kelimeler); //hash table a eklendi

                    balikHeap.Insert(balik); //heape eklendi.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("dosya okunurken hata oluştu " + e.Message);
            }
            return (balıklar, baliklarHashTable, balikHeap);
        }
        public static void Main(string[] args)
        {
            BinaryTree<EgeDeniziB> baliklarTree = DosyaOku().tree; //tree

            Console.WriteLine(baliklarTree.InOrderTraversal());
            Console.WriteLine($"Ortalama Derinlik: {EgeDeniziB.GetOrtalamaDerinlik()}");
            foreach (var balik in baliklarTree.InOrderDizi())
            {
                Console.WriteLine($"{balik.isim} - Dengeli Derinlik: {balik.dengeliDerinlik}");
            }

            Console.WriteLine("Lütfen iki harf giriniz.");
            string baslangic, son; //2 harf alindi
            baslangic = Console.ReadLine().ToUpper();
            son = Console.ReadLine().ToUpper();

            while (baslangic.Length != 1 || son.Length != 1)
            {
                Console.WriteLine("Tekrar Deneyin");
                baslangic = Console.ReadLine().ToUpper();
                son = Console.ReadLine().ToUpper();
            }
            char baslangic2 = Convert.ToChar(baslangic);
            char son2 = Convert.ToChar(son);
            baliklarTree.IkiHarf(baslangic2, son2);

            List<EgeDeniziB> siraliDizi = baliklarTree.InOrderDizi(); //liste olusturuldu 
            baliklarTree.DengeliAgacOlustur(siraliDizi); //listeden dengeli agac olusturuldu

            Hashtable baliklarHashTable = DosyaOku().hash;   //Hash 
            Console.WriteLine("Balık adı girin: "); //balik adi input
            string balikAdi = Console.ReadLine().ToUpper();

            while (!baliklarHashTable.ContainsKey(balikAdi))
            {
                Console.WriteLine("Lütfen tekrar balık adı girin: ");
                balikAdi = Console.ReadLine().ToUpper();
            }
            Console.WriteLine("Balık bilgisi girin: "); //bilgi input
            string yeniBilgi = Console.ReadLine().ToUpper();



            string[] yeniBilg = yeniBilgi.Split(new[] { ' ', ',', '.', ';', ':', '!', '?', '-', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            BinaryTree<string> yenikelimeler = new BinaryTree<string>();
            foreach (var item in yeniBilg.Where(w => !string.IsNullOrWhiteSpace(w))) //yeni bilgiyle kelime ağacı oluşturuluyor
            {
                yenikelimeler.Insert(item.ToLower());
            }
            baliklarHashTable[balikAdi] = yenikelimeler;  //girilen bilgi güncellendi

            BinaryTree<string> kelimeAgaci = (BinaryTree<string>)baliklarHashTable[balikAdi];  //Güncel kelime ağacını yazdır
            Console.WriteLine($"{balikAdi} -{kelimeAgaci.InOrderTraversal()}");
            Console.ReadKey();

            MaxHeap<EgeDeniziB> baliklarMaxHeap = DosyaOku().heap; //heap
            Console.WriteLine("Max heap elemanları");
            foreach (var item in baliklarMaxHeap.GetHeapList())
            {

                Console.WriteLine(item.isim);                   // heap listesi yazdırıldı
            }
            Console.ReadKey();

            var sayac = 0;
            foreach (var item in baliklarMaxHeap.GetHeapList())
            {
                Console.WriteLine(item.ToString());
                sayac++;
                if (sayac == 3) break;

            }
            Console.ReadKey();
            //100 elemanlı dizi
            int[] dizi = { 184, 26, 874, 369, 299, 838, 500, 244, 388, 451, 209, 118, 222, 92, 885, 787, 87, 139, 539, 263, 625, 507, 875, 545, 39, 18, 516, 633, 382, 868, 415, 610, 818, 39, 701, 455, 149, 323, 140, 59, 487, 573, 936, 142, 360, 473, 293, 689, 540, 248, 327, 408, 54, 710, 16, 776, 173, 843, 629, 806, 958, 688, 51, 811, 754, 236, 964, 878, 981, 35, 848, 422, 175, 396, 354, 647, 602, 153, 458, 53, 103, 887, 636, 909, 927, 123, 402, 152, 211, 498, 278, 245, 523, 395, 474, 660, 796, 417, 32, 848 };

            Stopwatch stopwatch = new Stopwatch(); //hassas zaman hesaplamak icin
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 10000000; i++)
            {
                BubbleSort(dizi);
            }
            stopwatch.Stop();
            Console.WriteLine($"Bubble sort algoritması için geçen süre : {stopwatch.Elapsed.TotalMilliseconds} ms ");

            stopwatch2.Start();
            for (int i = 0; i < 10000000; i++)
            {
                QuickSort(dizi);
            }
            stopwatch2.Stop();
            Console.WriteLine($"Quick sort algoritması için geçen süre : {stopwatch2.Elapsed.TotalMilliseconds} ms ");

        }
    }
}
