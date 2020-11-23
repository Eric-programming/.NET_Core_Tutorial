using Microsoft.AspNetCore.Identity;

namespace API_Advanced.Models
{
    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.identityuser?view=aspnetcore-1.1
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}