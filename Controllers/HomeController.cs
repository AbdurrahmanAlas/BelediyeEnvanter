using BelediyeEnvanter.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BelediyeEnvanter.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(new
    {
        Departments = await db.Departments.CountAsync(),
        Directorates = await db.Directorates.CountAsync(),
        Rooms = await db.Rooms.CountAsync(),
        Assets = await db.Assets.CountAsync()
    });
}
