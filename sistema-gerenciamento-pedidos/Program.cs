using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Services.Cliente;
using sistema_gerenciamento_pedidos.Services.Cliente.Interfaces;
using sistema_gerenciamento_pedidos.Services.EnderecoCliente;
using sistema_gerenciamento_pedidos.Services.EnderecoCliente.Interface;
using sistema_gerenciamento_pedidos.Services.EnderecoFuncionario;
using sistema_gerenciamento_pedidos.Services.EnderecoFuncionario.Interfaces;
using sistema_gerenciamento_pedidos.Services.Funcionarios;
using sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces;
using sistema_gerenciamento_pedidos.Services.Pedidos;
using sistema_gerenciamento_pedidos.Services.Pedidos.Interfaces;
using sistema_gerenciamento_pedidos.Services.Produtos;
using sistema_gerenciamento_pedidos.Services.Produtos.Interfaces;
using sistema_gerenciamento_pedidos.Services.Senha;
using sistema_gerenciamento_pedidos.Services.Senha.Interface;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//MAPEAMENTO DAS INJEÇÕES DE DEPENDÊNCIAS
builder.Services.AddScoped<ISenhaService, SenhaService>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IEnderecoClienteService, EnderecoClienteService>();
builder.Services.AddScoped<IEnderecoFuncionarioService, EnderecoFuncionarioService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddHangfire(x =>
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddControllers()

.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Adiciona permissão de Cors para que o front-end tenha acesso a API 
builder.Services.AddCors(options =>
{
    options.AddPolicy("sistema-gerenciamento-pedidos-site", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("sistema-gerenciamento-pedidos-site");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();
