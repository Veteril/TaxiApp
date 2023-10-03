using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Extentions
{
    public static class PrepDb
    {
        public static void UseMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
