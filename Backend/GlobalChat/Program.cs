using GlobalChat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "https://hosting.d319zccr7w86h1.amplifyapp.com") // Replace with your React.js client's URL
                            .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();

                });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting(); // Add UseRouting before other middleware

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.UseCors("AllowSpecificOrigin");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<Chathub>("/chatHubA"); // Ensure this mapping is correct
});

app.Run();
