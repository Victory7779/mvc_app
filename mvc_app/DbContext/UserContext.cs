using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


    public class UserContext:Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public UserContext(DbContextOptions<UserContext> options) 
            : base(options) { }
    }

