using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    public class KursKayit
    {
        [Display(Name ="Kurs Kayıt No")]
        public int KursKayitId { get; set; }

        [Display(Name ="Öğrenci Adı-Soyadı")]
        public int OgrenciId { get; set; }
        public Ogrenci Ogrenci { get; set; } = null!; // Data/Ogrenci.cs dosyasında ki Ogrenci Bilgilerini Çekiyoruz = Null! Boş değil

        public int KursId { get; set; }
        public Kurs Kurs{ get; set; } = null!; // Data/Kurs.cs dosyasında ki Kurs Bilgilerini Çekiyoruz = Null! Boş değil

        public DateTime KayitTarihi { get; set; }
    }
}