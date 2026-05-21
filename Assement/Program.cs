using Assessment.Infrastructure.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Assessment.APIs // اتأكد إن الـ Namespace مطابق لاسم مشروع الـ API عندك
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. تسجيل الـ Controllers الخاصة بالـ Web API بدلاً من الـ Razor Pages
            builder.Services.AddControllers();

            // 2. تسجيل الـ DbContext لربطه بقاعدة البيانات (اللي عملناه الخطوة اللي فاتت)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 3. تفعيل الـ Swagger عشان الـ HR والـ Technical Lead يقدروا يختبروا الـ APIs بسهولة
            builder.Services.AddEndpointsApiExplorer();
            object value = builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // 4. إعدادات بيئة التطوير وتشغيل الـ Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // 5. تأمين الـ APIs (الـ Authentication لازم تسبق الـ Authorization)
            app.UseAuthentication();
            app.UseAuthorization();

            // 6. توجيه الـ Requests للـ Controllers
            app.MapControllers();

            app.Run();
        }
    }
}