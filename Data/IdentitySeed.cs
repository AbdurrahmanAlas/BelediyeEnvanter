using Microsoft.AspNetCore.Identity;

namespace BelediyeEnvanter.Data;
public static class IdentitySeed
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roles=services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach(var role in new[]{"Yönetici","Personel"}) if(!await roles.RoleExistsAsync(role)) await roles.CreateAsync(new IdentityRole(role));
        var users=services.GetRequiredService<UserManager<IdentityUser>>();
        var admin=await users.FindByNameAsync("admin");
        if(admin==null){admin=new IdentityUser{UserName="admin",Email="admin@belediye.local",EmailConfirmed=true};var result=await users.CreateAsync(admin,"Admin1234");if(result.Succeeded)await users.AddToRoleAsync(admin,"Yönetici");}
    }
}
