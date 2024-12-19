using Name2;

namespace Name
{  
    class EgeDeniziB : IComparable<EgeDeniziB>
    {
        public string isim;
        public string bilgi;
        public string[] words;
        public BinaryTree<string> kelimeler; 
        public static int toplamDerinlik=0;
        public EgeDeniziB(string isim,string info){
            this.isim=isim;
            bilgi=info;
            words=bilgi.Split(new[] { ' ', ',', '.', ';', ':', '!', '?','-', '(' ,')' }, StringSplitOptions.RemoveEmptyEntries);
            kelimeler=new BinaryTree<string>();

            foreach (var item in words.Where(w => !string.IsNullOrWhiteSpace(w)))
            {
                kelimeler.Insert(item.ToLower());// tree olustu
            }
        }

        public int CompareTo(EgeDeniziB other)
        {
            if (other == null) return 1;
            return isim.CompareTo(other.isim); 
        }
        public override string ToString()
        {
            return $"{kelimeler.InOrderTraversal()} - {isim} \n ";
        }

        //agac derinligi hesaplama methodu 
        //dugum sayilarini bulan+dengeli olmasi icin derinligin kac olmasi gerektigini hesaplayan method
    }

    class Program
    {
        public static BinaryTree<EgeDeniziB> DosyaOku(){ //dosya okundu ve tree oluşturuldu
            string projeYolu = Directory.GetCurrentDirectory(); 
            string dosyaAdi = "baliklar.txt";
            string dosyaYolu = Path.Combine(projeYolu, dosyaAdi); 
            BinaryTree<EgeDeniziB> balıklar= new BinaryTree<EgeDeniziB>();
            try
            {
                foreach(var line in File.ReadAllLines(dosyaYolu)){
                    var info=line.Split("/");
                    EgeDeniziB balik=new EgeDeniziB(info[0],info[1]); 
                    balıklar.Insert(balik);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("dosya okunurken hata oluştu " + e.Message);
            }
            return balıklar;
        }
        public static void Main(string[] args){
            BinaryTree<EgeDeniziB> baliklarTree= DosyaOku();
            baliklarTree.InOrderTraversal();
        }
    }
    
}