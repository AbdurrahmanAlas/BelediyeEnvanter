using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace BelediyeEnvanter.Data;
public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext(options) { }
