using BelediyeEnvanter.Data;
using BelediyeEnvanter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BelediyeEnvanter.Controllers;
public class RoomsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? bina,string? kat,int? mudurlukId){var q=db.Rooms.Include(x=>x.Directorate).ThenInclude(x=>x!.Department).Include(x=>x.Assets).AsQueryable();if(!string.IsNullOrWhiteSpace(bina))q=q.Where(x=>x.Building.Contains(bina));if(!string.IsNullOrWhiteSpace(kat))q=q.Where(x=>x.Floor==kat);if(mudurlukId.HasValue)q=q.Where(x=>x.DirectorateId==mudurlukId);ViewBag.Mudurlukler=await db.Directorates.OrderBy(x=>x.Name).ToListAsync();return View(await q.OrderBy(x=>x.Building).ThenBy(x=>x.RoomNumber).ToListAsync());
    }
    public async Task<IActionResult> Create() { ViewBag.Mudurlukler = await db.Directorates.Include(x => x.Department).OrderBy(x => x.Name).ToListAsync(); return View(); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(Room oda) { if (!ModelState.IsValid) { ViewBag.Mudurlukler = await db.Directorates.Include(x => x.Department).ToListAsync(); return View(oda); } db.Add(oda); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int id){var o=await db.Rooms.FindAsync(id);if(o==null)return NotFound();ViewBag.Mudurlukler=await db.Directorates.Include(x=>x.Department).ToListAsync();return View(o);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(Room oda){if(!ModelState.IsValid){ViewBag.Mudurlukler=await db.Directorates.Include(x=>x.Department).ToListAsync();return View(oda);}db.Update(oda);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    public async Task<IActionResult> Delete(int id){var o=await db.Rooms.Include(x=>x.Assets).FirstOrDefaultAsync(x=>x.Id==id);if(o!=null&&o.Assets.Count==0){db.Remove(o);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
