using AnketPortali.Models;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt.Extensions;

namespace AnketPortali.Models
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<SurveyApplication> SurveyApplications { get; set; }

        public string MD5Hash(string pass)
        {
            var salt = _config.GetValue<string>("AppSettings:MD5Salt");
            var password = pass + salt;
            var hashed = password.MD5();
            return hashed;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<User>()
                .HasData(
                new User {Id=1,Username = "nil", Email = "nil@gmail.com",PasswordHash=MD5Hash("nil123"),Role="admin"}
                );
   
            modelBuilder.Entity<SurveyApplication>()
                   .HasOne(sa => sa.User)
                   .WithMany()
                   .HasForeignKey(sa => sa.UserId);

            modelBuilder.Entity<SurveyApplication>()
                .HasOne(sa => sa.Survey)
                .WithMany()
                .HasForeignKey(sa => sa.SurveyId);

            base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
