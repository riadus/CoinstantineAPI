using System;
using CoinstantineAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace CoinstantineAPI.WebApi.Middleware.Attributes
{
    public class AuthorizeManagerAttribute : AuthorizeAttribute
    {
        public AuthorizeManagerAttribute()
        {
            Roles = $"{UserRole.Admin},{UserRole.Manager}";
        }
    }
}
