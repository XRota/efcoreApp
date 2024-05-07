using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    public class Ogretmen
    {
        [Display(Name ="Öğretmen No")]

        public int OgretmenId { get; set; }
        [Display(Name ="Adı")]

        public string? Ad { get; set; }
        [Display(Name ="Soyadı")]

        public string? SoyAd { get; set; }
        
        [Display(Name ="Öğretmen")]
        public string AdSoyad { 
        get{
            return this.Ad + " " + this.SoyAd;
        }}
        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}",ApplyFormatInEditMode =false)]
        public DateTime BaslamaTarihi {get;set;}

        public ICollection<KursKayit> Kurslar { get; set; } = new List<KursKayit>();

    }
}