using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    public class Kurs
    {
        [Display(Name ="Kurs No")]
        public int KursId { get; set; }
        [Display(Name ="Kurs Başlığı")]
        public string? Baslik { get; set; }

        
        public int OgretmenId { get; set; }

        
        public Ogretmen Ogretmen { get; set; }=null!;

        public ICollection<KursKayit> kurskayit { get; set; } = new List<KursKayit>();

    }
}