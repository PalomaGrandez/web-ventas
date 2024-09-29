using Entradas.Server.Services.EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Agregamos Swagger para generar la pagina del API
builder.Services.AddEndpointsApiExplorer();//Swagger
builder.Services.AddSwaggerGen();//Swagger

//Agregamos los servicios para operaciones con la BD
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IEventoEntradaService, EventoEntradaService>();
builder.Services.AddScoped<IEventoFechaService, EventoFechaService>();
builder.Services.AddScoped<IOrdenService, OrdenService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();//Swagger
app.UseSwaggerUI();//Swagger

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
