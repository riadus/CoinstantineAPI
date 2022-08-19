using CoinstantineAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace CoinstantineAPI.WebApi.Middleware.Attributes
{
    public class AuthorizeAdminOnlyAttribute : AuthorizeAttribute
    {
        public AuthorizeAdminOnlyAttribute()
        {
            Roles = UserRole.Admin.ToString();
        }
    }
}
