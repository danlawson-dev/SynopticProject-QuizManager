using Microsoft.EntityFrameworkCore;
using QuizManager.DataAccess;
using QuizManager.DataAccess.Data;
using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;
using QuizManager.Models.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuizManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:QuizManagerDbContext"]));

builder.Services.AddDefaultIdentity<User>(options => 
    options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<QuizManagerDbContext>();

// Add the user permission levels as policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserPolicyConstants.RestrictedPolicy, policy =>
        policy.RequireClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.Restricted));

    // User with "Edit" permissions level is also part of the "ViewPolicy"
    options.AddPolicy(UserPolicyConstants.ViewPolicy, policy =>
        policy.RequireClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.View, UserPermissionLevelClaimConstants.Edit));

    options.AddPolicy(UserPolicyConstants.EditPolicy, policy =>
        policy.RequireClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.Edit));
});

// Add the interfaces to access the data repositories
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

// Bundle all CSS files into one file
builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddBundle("/css/bundle.css",
        "text/css; charset=UTF-8",
        "/css/accordion.css",
        "/css/button.css",
        "/css/footer.css",
        "/css/form.css",
        "/css/layout-common.css",
        "/css/layout-external.css",
        "/css/layout-internal.css",
        "/css/page-heading.css"
    ).Concatenate();
});

var app = builder.Build();

// Create database if it does not already exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<QuizManagerDbContext>();

    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=List}");

app.Run();