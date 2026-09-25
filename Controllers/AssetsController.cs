using BelediyeEnvanter.Data;
using BelediyeEnvanter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BelediyeEnvanter.Controllers;

public class AssetsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? q, string? kategori, string? durum, int? odaId, string? personel) { q=q?.Trim(); kategori=kategori?.Trim(); durum=durum?.Trim(); personel=personel?.Trim(); var query=db.Assets.Include(x=>x.Room).ThenInclude(x=>x!.Directorate).AsQueryable(); if(!string.IsNullOrWhiteSpace(q)){var arama=q.ToLower();query=query.Where(x=>x.Name.ToLower().Contains(arama)||x.SerialNumber.ToLower().Contains(arama)||x.InventoryNumber.ToLower().Contains(arama)||x.KullananPersonel.ToLower().Contains(arama)||x.ResponsiblePerson.ToLower().Contains(arama)||x.BrandModel.ToLower().Contains(arama));} if(!string.IsNullOrWhiteSpace(kategori)) query=query.Where(x=>x.Category==kategori); if(!string.IsNullOrWhiteSpace(durum)) query=query.Where(x=>x.Status==durum); if(odaId.HasValue) query=query.Where(x=>x.RoomId==odaId); if(!string.IsNullOrWhiteSpace(personel)) query=query.Where(x=>x.KullananPersonel==personel); ViewBag.Kategoriler=await db.Assets.Select(x=>x.Category).Where(x=>x!="").Distinct().OrderBy(x=>x).ToListAsync(); ViewBag.Odalar=await db.Rooms.OrderBy(x=>x.Building).ThenBy(x=>x.RoomNumber).ToListAsync(); ViewBag.Personeller=await db.Assets.Select(x=>x.KullananPersonel).Where(x=>x!="").Distinct().OrderBy(x=>x).ToListAsync(); ViewBag.SecilenPersonel=personel; ViewBag.SecilenKategori=kategori; ViewBag.SecilenDurum=durum; ViewBag.SecilenOda=odaId; ViewBag.Arama=q; return View(await query.OrderBy(x=>x.Name).ToListAsync()); }
    public async Task<IActionResult> Create() { ViewBag.Rooms=await db.Rooms.Include(x=>x.Directorate).ToListAsync(); ViewBag.KategoriListesi=await db.Categories.OrderBy(x=>x.Name).ToListAsync(); return View(); }
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Asset asset) { if(!ModelState.IsValid){ViewBag.Rooms=await db.Rooms.Include(x=>x.Directorate).ToListAsync();ViewBag.KategoriListesi=await db.Categories.OrderBy(x=>x.Name).ToListAsync();return View(asset);} db.Add(asset);await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int id){var a=await db.Assets.FindAsync(id);if(a==null)return NotFound();ViewBag.Rooms=await db.Rooms.Include(x=>x.Directorate).ToListAsync();ViewBag.KategoriListesi=await db.Categories.OrderBy(x=>x.Name).ToListAsync();return View(a);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(Asset asset){if(!ModelState.IsValid){ViewBag.Rooms=await db.Rooms.Include(x=>x.Directorate).ToListAsync();ViewBag.KategoriListesi=await db.Categories.OrderBy(x=>x.Name).ToListAsync();return View(asset);}db.Update(asset);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    public async Task<IActionResult> Delete(int id){var a=await db.Assets.FindAsync(id);if(a!=null){db.Remove(a);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
