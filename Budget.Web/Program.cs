using Budget.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. AJOUT DU SUPPORT MVC
builder.Services.AddControllersWithViews(); 
builder.Services.AddScoped<IBudgetLines, BudgetLines>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

// 2. ACTIVATION DES CONTROLLERS
app.MapControllers(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expenses}/{action=Index}/{id?}");

app.Run();