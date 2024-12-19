using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            this.isim = isim;
            bilgi = info;
            words = bilgi.Split(new[] { ' ', ',', '.', ';', ':', '!', '?','-' ,'(',')'}, StringSplitOptions.RemoveEmptyEntries);
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
            return ($" {isim} - {kelimeler.InOrderTraversal()} \n");
        }


        public static int GetOrtalamaDerinlik()
        {
            return toplamDerinlik / 38;
        }



        //dugum sayilarini bulan+dengeli olmasi icin derinligin kac olmasi gerektigini hesaplayan method
    }
    class Program
    {
        public static BinaryTree<EgeDeniziB> DosyaOku()
        { //dosya okundu ve tree oluşturuldu
            string projeYolu = Directory.GetCurrentDirectory();
            string dosyaAdi = "baliklar.txt";
            string dosyaYolu = Path.Combine(projeYolu, dosyaAdi);
            BinaryTree<EgeDeniziB> balıklar = new BinaryTree<EgeDeniziB>();
            try
            {
                foreach (var line in File.ReadAllLines(dosyaYolu))
                {
                    var info = line.Split('/');
                    EgeDeniziB balik = new EgeDeniziB(info[0], info[1]);
                    balıklar.Insert(balik);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("dosya okunurken hata oluştu " + e.Message);
            }
            return balıklar;
        }
        public static void Main(string[] args)
        {
            BinaryTree<EgeDeniziB> baliklarTree = DosyaOku();
            baliklarTree.InOrderTraversal();
            Console.ReadKey();
            Console.WriteLine("Lütfen iki harf giriniz.");
            
            string baslangic, son;
            baslangic = Console.ReadLine().ToUpper();
            son = Console.ReadLine().ToUpper();

            while(baslangic.Length != 1 || son.Length != 1)
            {
                Console.WriteLine("Tekrar Deneyin");
                baslangic = Console.ReadLine().ToUpper();
                son = Console.ReadLine().ToUpper();
                
            }
            char baslangic2 = Convert.ToChar(baslangic);
            char son2 = Convert.ToChar(son);

            baliklarTree.IkiHarf(baslangic2, son2);

            Console.ReadKey();
            
        }
    }
}
