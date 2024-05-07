using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using efcoreApp.Data;

namespace efcoreApp.Models
{
    public class KursViewModel
    {
        public int KursId { get; set; }
        [Required]
        [Display(Name ="Kurs Başlığı")]

        public string? Baslik { get; set; }

        public int OgretmenId { get; set; }
        public ICollection<KursKayit> kurskayit { get; set; } = new List<KursKayit>();


        

    }
}