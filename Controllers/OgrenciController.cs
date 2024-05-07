using System.Net.Http.Headers;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;

namespace efcoreApp.Controllers
{
    public class OgrenciController:Controller
    {
        // DataContext sınıfı, veritabanı bağlamınızı temsil eder ve Entity Framework Core tarafından kullanılır.
        private readonly DataContext _context;
        // Controller sınıfının yapıcı metodu. Dependency Injection yoluyla DataContext nesnesini alır.
        public OgrenciController (DataContext context)
        {
            // Alınan DataContext nesnesi, özel alana atanır.
            _context = context;
        }
        public async Task<IActionResult> Index() //async Task asekron gönderimlerde kullanılıyor
        {            
            return View(await _context.Ogrenciler.ToListAsync()); // await asekron metodun da olması gereken bekletme kodu
        }
        public IActionResult Create(){
             // Bu metod çağrıldığında, 'Create' adında bir View döndürülür.
             // MVC, 'Create' adında bir .cshtml dosyasını Views klasöründe arar.
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (Ogrenci model)
        {
            _context.Ogrenciler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
         [HttpGet]
        public async Task<IActionResult> Edit(int? id) //Edit Butonu
        {
            if(id==null)
            {
                return NotFound();
            }
            var ogr = await _context
            .Ogrenciler
            .Include(o => o.KursKayitlari)
            .ThenInclude(o => o.Kurs)
            .FirstOrDefaultAsync(o => o.OgrenciId==id); 
            // Alternatif Arama Yöntemi
            // var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);


            if (ogr == null)
            {
                return NotFound();
            }
            return View(ogr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Güvenlik önlemi şifreli bir şekilde verinin doğru yerden gelip gelmediğine bakar
        public async Task<IActionResult> Edit(int id,Ogrenci model)
        {
            if(id != model.OgrenciId) //gönderiler Id ile Veritabanındaki Id aynı değil ise
            {
                return NotFound(); // 404 hata sayfasına yönlendir.
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync(); //Veri tabanına aktarmak.
                }
                catch(DbUpdateConcurrencyException)
                {
                    //Any herhangi bir demek. Bir yada daha fazla kayıt varmı sorgusunda kullanılıyor.
                    if(!_context.Ogrenciler.Any(o => o.OgrenciId == model.OgrenciId))
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
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ogr =await _context.Ogrenciler.FindAsync(id);
            if(ogr == null)
            {
                return NotFound();
            }
            return View(ogr);
        }
        [HttpPost]
        public async Task<IActionResult> Delete ([FromForm]int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if(ogrenci==null)
            {
                return NotFound();
            }
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}