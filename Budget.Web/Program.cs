using Budget.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURATION OF THE SERVICES (Before the build) ---
builder.Services.AddControllersWithViews(); 
builder.Services.AddScoped<IBudgetLines, BudgetLines>();
builder.Services.AddScoped<IPayments, Payments>();
builder.Services.AddScoped<IExpenseTemplates, ExpenseTemplates>();
builder.Services.AddScoped<IExpenses, Expenses>();

var mvcBuilder = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

// --- END CONFIGURATION ----------------------------------
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
app.MapControllers(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expenses}/{action=Index}/{id?}");

app.Run();