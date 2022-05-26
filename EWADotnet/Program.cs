using EWA.Sugar;
using EWADotnet.Authorize;
using Furion;


var builder = WebApplication.CreateBuilder(args).Inject();

// Add services to the container.
builder.Services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//×¢²áJwt ,enableGlobalAuthorizeÈ«¾ÖÊÚÈ¨
builder.Services.AddCorsAccessor();//¿çÓò
builder.Services.AddControllers().AddInject();
builder.Services.AddSqlsugarSetup(App.Configuration);//×¢²áSqlSugar

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();//¿çÓò
app.UseCorsAccessor();//¿çÓò

app.UseAuthentication();
app.UseAuthorization();

app.UseInject();

app.MapControllers();

app.Run();
