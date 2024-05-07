using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController:Controller
    {
        private readonly DataContext _context;
        public KursController (DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.Kurslar
            .Include(k =>k.Ogretmen)
            .ToListAsync();
            return View(kurslar);
        }

        public async Task<IActionResult> Create(){
            ViewBag.Ogretmenler =new SelectList( await _context.Ogretmenler.ToListAsync(), "OgretmenId","AdSoyad");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (KursViewModel model)
        {
            if(ModelState.IsValid)
            {
                _context.Kurslar.Add(new Kurs(){KursId=model.KursId, Baslik=model.Baslik, OgretmenId=model.OgretmenId});
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            }
            
            ViewBag.Ogretmenler =new SelectList( await _context.Ogretmenler.ToListAsync(), "OgretmenId","AdSoyad");
            return View(model);
        }

        //------------------------------------------------------Edit Get - Post Kısmı
         [HttpGet]
        public async Task<IActionResult> Edit(int? id) //Edit Butonu
        {
            if(id==null)
            {
                return NotFound();
            }
            var krs = await _context
            .Kurslar
            .Include(k => k.kurskayit)
            .ThenInclude(k => k.Ogrenci)
            .Select(k => new KursViewModel{
                KursId=k.KursId,
                Baslik=k.Baslik,
                OgretmenId=k.OgretmenId,
                kurskayit=k.kurskayit
            })
            .FirstOrDefaultAsync(k => k.KursId == id); 
            // Alternatif Arama Yöntemi
            // var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);


            if (krs == null)
            {
                return NotFound();
            }

            ViewBag.Ogretmenler =new SelectList( await _context.Ogretmenler.ToListAsync(), "OgretmenId","AdSoyad");

            return View(krs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Güvenlik önlemi şifreli bir şekilde verinin doğru yerden gelip gelmediğine bakar
        public async Task<IActionResult> Edit(int id,KursViewModel model)
        {
            if(id != model.KursId) //gönderiler Id ile Veritabanındaki Id aynı değil ise
            {
                return NotFound(); // 404 hata sayfasına yönlendir.
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Kurs(){KursId=model.KursId, Baslik=model.Baslik, OgretmenId=model.OgretmenId});
                    await _context.SaveChangesAsync(); //Veri tabanına aktarmak.
                }
                catch(DbUpdateConcurrencyException)
                {
                    //Any herhangi bir demek. Bir yada daha fazla kayıt varmı sorgusunda kullanılıyor.
                    if(!_context.Ogrenciler.Any(o => o.OgrenciId == model.KursId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; //kaldığın yerden devam et.
                    }
                }
                return RedirectToAction("Index"); //Güncellemeyi Geriye gönder.
            }
            ViewBag.Ogretmenler =new SelectList( await _context.Ogretmenler.ToListAsync(), "OgretmenId","AdSoyad");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var krs =await _context.Kurslar.FindAsync(id);
            if(krs == null)
            {
                return NotFound();
            }
            return View(krs);
        }

        //-------------------------------------------------------------Edit Son

        //-------------------------------------------------------------Delete
        [HttpPost]
        public async Task<IActionResult> Delete ([FromForm]int id)
        {
            var krs = await _context.Kurslar.FindAsync(id);
            if(krs==null)
            {
                return NotFound();
            }
            _context.Kurslar.Remove(krs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //-------------------------------------------------------------Delete Son

    }
}