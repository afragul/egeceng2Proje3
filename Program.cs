using System;
using System.Collections;
using System.Collections.Generic;
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
        private int dengeliDerinlik;

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
            derinlik = kelimeler.CalculateDepth().depht;
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

            //BinaryTree<string> kelimeAgaci = (BinaryTree<string>)baliklarHashTable[balikAdi];  //Güncel kelime ağacını yazdır
            //Console.WriteLine($"{balikAdi} -{kelimeAgaci.InOrderTraversal()}");
            //Console.ReadKey();

            MaxHeap<EgeDeniziB> baliklarMaxHeap = DosyaOku().heap; //heap
            //Console.WriteLine("Max heap elemanları");
            //foreach (var item in baliklarMaxHeap.GetHeapList())
            //{

            //    Console.WriteLine(item.isim);                   // heap listesi yazdırıldı
            //}
            //Console.ReadKey();

            var sayac = 0;
            foreach(var item in baliklarMaxHeap.GetHeapList())
            {
                Console.WriteLine(item.ToString());
                sayac++;
                if (sayac == 3) break;

            }
            Console.ReadKey();
        }
    }
}
