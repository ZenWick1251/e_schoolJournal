using e_journal.Data;
using e_journal.Interfaces;
using e_journal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<JournalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(JournalMapping));


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStudentService, StudentSevice>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ILessonScheduleService, LessonScheduleService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ISchoolClassService, SchoolCLassService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "eJournal API", Version = "v1" });
    c.AddSecurityDefinition("X-UserId", new OpenApiSecurityScheme
    {
        Name = "UserId",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "ID текущего пользователя"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "UserId",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "UserId"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JournalDbContext>();

    if (!db.Users.Any(u => u.Role == "Admin"))
    {
        db.Users.Add(new e_journal.Models.User
        {
            Username = "admin",
            Password = "admin123", 
            Role = "Admin"
        });

        db.SaveChanges();
    }
}



app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
