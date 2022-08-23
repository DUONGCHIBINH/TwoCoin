
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
//using TwoCoinApi;
//using YViet_EMR.Model;


//DBStatic db1 = new DBStatic();
//DBStatic db2 = new DBStatic();
//DBStatic db3 = new DBStatic();
//DBStatic db4 = new DBStatic();
//DBStatic db5 = new DBStatic();

//void inchu(string chu, DBStatic db)
//{
//    for (int i = 0; i < 10; i++)
//    {
//        //Debug.WriteLine(chu + " - " + DateTime.Now);
//        //Thread.Sleep(1000);
//        var temp = db.Repository.GetQuery<Book_Online>();
//        Debug.WriteLine(chu + " - " + temp.Count());
//        Thread.Sleep(1000);
//    }

//}

//Debug.WriteLine("Bat dau");
//var a = new Thread(() => inchu("A", db1));
//var b = new Thread(() => inchu("B", db2));
//var c = new Thread(() => inchu("C", db3));
//var d = new Thread(() => inchu("D", db4));
//var e = new Thread(() => inchu("E", db5));


//a.Start();
//b.Start();
//c.Start();
//d.Start();
//e.Start();

//Thread.Sleep(10000);
//Debug.WriteLine("Ket thuc");

//Console.ReadLine();



//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();




Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<TwoCoinApi.Startup>();
               }).Build().Run();
