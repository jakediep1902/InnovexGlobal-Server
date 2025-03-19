var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ Swagger để hiển thị API trong UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Thêm CORS để cho phép các trang web khác gọi API này
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin() // Cho phép mọi domain truy cập API
                        .AllowAnyMethod()  // Cho phép tất cả các phương thức (GET, POST, PUT, DELETE)
                        .AllowAnyHeader()); // Cho phép tất cả các header
});

var app = builder.Build();

// Kích hoạt CORS để có thể gọi API từ trình duyệt
app.UseCors("AllowAll");

// Chỉ bật Swagger khi ở chế độ Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Bật chuyển hướng HTTPS (có thể tắt nếu không cần)
//app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


// Danh sách các điều kiện thời tiết mẫu
var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

// API 1: Trả về danh sách thời tiết ngẫu nhiên
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)), // Ngày hiện tại + index ngày
            Random.Shared.Next(-20, 55), // Nhiệt độ ngẫu nhiên từ -20 đến 55 độ C
            summaries[Random.Shared.Next(summaries.Length)] // Chọn ngẫu nhiên trạng thái thời tiết
        )).ToArray();

    return Results.Ok(forecast); // Trả về kết quả dạng JSON
})
.WithName("GetWeatherForecast") // Đặt tên cho API
.WithOpenApi(); // Để hiển thị trong Swagger UI

// API 2: Trả về chuỗi văn bản đơn giản
app.MapGet("/hello", () => "Hello from API!")
    .WithName("GetHello") // Đặt tên cho API
    .WithOpenApi(); // Để hiển thị trong Swagger UI

app.Run(); // Chạy ứng dụng

// Định nghĩa kiểu dữ liệu WeatherForecast
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); // Chuyển đổi từ độ C sang độ F
}
