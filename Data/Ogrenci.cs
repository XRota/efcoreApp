using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace efcoreApp.Data
{
    public class Ogrenci
    {
        // id => primary key (Birincil Anahtar) olmak zorunda
        [Key]
        [Display(Name ="Öğrenci no")]
        public int OgrenciId { get; set; }
        [Display(Name ="Adı")]
        public string? OgrenciAd { get; set; }
        [Display(Name ="Soyadı")]
        public string? OgrenciSoyad { get; set; }

        // İki nesneyi alıp birleştiriyoruz
        public string AdSoyad { 
        get{
            return this.OgrenciAd + " " + this.OgrenciSoyad;
        }}

        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();
    }
}