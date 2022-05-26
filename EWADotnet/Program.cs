using EWA.Sugar;
using EWADotnet.Authorize;
using Furion;


var builder = WebApplication.CreateBuilder(args).Inject();

// Add services to the container.
builder.Services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//ע��Jwt ,enableGlobalAuthorizeȫ����Ȩ
builder.Services.AddCorsAccessor();//����
builder.Services.AddControllers().AddInject();
builder.Services.AddSqlsugarSetup(App.Configuration);//ע��SqlSugar

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();//����
app.UseCorsAccessor();//����

app.UseAuthentication();
app.UseAuthorization();

app.UseInject();

app.MapControllers();

app.Run();
