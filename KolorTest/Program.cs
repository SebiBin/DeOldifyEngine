using System.Diagnostics;
using DeOldifyEngine;
using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);
var configuration = builder.Build();
var appSettings = configuration.GetSection("AppSettings");

var modelsPath = appSettings["ModelsPath"];
if (modelsPath == null)
{
    Console.WriteLine($"AppSettings / ModelsPath is empty. Please declare it.");
    return;
}

var exeDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var modelsDirectoryPath = Path.GetFullPath(Path.Combine(exeDirectoryPath, modelsPath));

if (!Directory.Exists(modelsDirectoryPath))
{
    Console.WriteLine($"{modelsDirectoryPath} not exists.");
    return;
}

var modelFiles = Directory.GetFiles(modelsDirectoryPath, "*.*model"); 
if (!modelFiles.Any())
{
    Console.WriteLine($"[{modelsDirectoryPath}] does not contains model files.");
    return;
}

Console.WriteLine($"Model files:");
Console.WriteLine($"{modelFiles.Aggregate("", (accumulator, piece) => accumulator + "\r\n" + piece)}");

var deOldifyEngine = new DeOldify();  

Console.WriteLine("Initialize DeOldify engine...");
deOldifyEngine.Initialize(modelsDirectoryPath);
Console.WriteLine("Initialized.");

var stopWatch = new Stopwatch();
var filesToConvert = Directory.GetFiles(exeDirectoryPath, @"TestFiles/*.jpg");

Console.WriteLine("");
Console.WriteLine($"Colorize files in: {exeDirectoryPath}TestFiles/");
foreach (var fileName in filesToConvert)
{
    var fileInfo = new FileInfo(fileName);
    var colorFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name}-color{fileInfo.Extension}");
    Console.WriteLine($"Colorize: {fileInfo.Name}");
    
    stopWatch.Start();
    deOldifyEngine.Colorize(fileName, colorFile);
    stopWatch.Stop();
    
    Console.WriteLine($"Elapsed time {stopWatch.Elapsed}\t  for {fileName}");
}














