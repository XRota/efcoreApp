using System.Net.Http.Headers;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;

namespace efcoreApp.Controllers
{
    public class OgretmenController:Controller
    {
        private readonly DataContext _context;
        public OgretmenController (DataContext context)
        {
            // Alınan DataContext nesnesi, özel alana atanır.
            _context = context;
        }
        public async Task<IActionResult> Index() //async Task asekron gönderimlerde kullanılıyor
        {            
            return View(await _context.Ogretmenler.ToListAsync()); // await asekron metodun da olması gereken bekletme kodu
        }
         public IActionResult Create(){
             // Bu metod çağrıldığında, 'Create' adında bir View döndürülür.
             // MVC, 'Create' adında bir .cshtml dosyasını Views klasöründe arar.
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (Ogretmen model)
        {
            _context.Ogretmenler.Add(model);
            // model.BaslamaTarihi=DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        //---------------------------------------------Edit ve Delete
         [HttpGet]
        public async Task<IActionResult> Edit(int? id) //Edit Butonu
        {
            if(id==null)
            {
                return NotFound();
            }
            var entity = await _context
            .Ogretmenler            
            .FirstOrDefaultAsync(o => o.OgretmenId==id); 
            // Alternatif Arama Yöntemi
            // var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);


            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Güvenlik önlemi şifreli bir şekilde verinin doğru yerden gelip gelmediğine bakar
        public async Task<IActionResult> Edit(int id,Ogretmen model)
        {
            if(id != model.OgretmenId) //gönderiler Id ile Veritabanındaki Id aynı değil ise
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
                    if(!_context.Ogretmenler.Any(o => o.OgretmenId == model.OgretmenId))
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
            var entity =await _context.Ogretmenler.FindAsync(id);
            if(entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Delete ([FromForm]int id)
        {
            var ogretmen = await _context.Ogretmenler.FindAsync(id);
            if(ogretmen==null)
            {
                return NotFound();
            }
            _context.Ogretmenler.Remove(ogretmen);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //---------------------------------------------Edit ve Delete Son

    }    
}