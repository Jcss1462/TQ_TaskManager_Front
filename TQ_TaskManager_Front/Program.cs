using TQ_TaskManager_Front.Services;

var builder = WebApplication.CreateBuilder(args);


// Agregar acceso a configuraci�n de appsettings.json
var configuration = builder.Configuration;

// Registro de IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Configurar HttpClient y AuthService
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    var baseUrl = configuration["ApiSettings:TQBackUrl"];
    client.BaseAddress = new Uri(baseUrl ?? throw new ArgumentNullException(nameof(baseUrl)));
})
//configuro HttpClient para ignorar la validaci�n del certificado
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});


// Add services to the container.
builder.Services.AddRazorPages();

// Agregar la configuraci�n de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true;                  // Evita el acceso a la cookie desde JavaScript
    options.Cookie.IsEssential = true;               // Requerido por GDPR
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Redirigir seg�n estado de autenticaci�n
app.MapGet("/", async context =>
{
    if (1==1)
    {
        context.Response.Redirect("/Account/Login");
    }
    else
    {
        context.Response.Redirect("/Dashboard");
    }
});

// Usar el middleware de sesiones
app.UseSession();

app.MapRazorPages();

app.Run();
