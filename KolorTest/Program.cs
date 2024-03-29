﻿using System.Diagnostics;
using DeOldifyEngine;
using System.Reflection;
using Microsoft.Extensions.Configuration;

//################################################################################################################
// ### PREPARE ###
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
Console.WriteLine($"");
//################################################################################################################

//################################################################################################################
// ### PREPARE ENGINE ###
//################################################################################################################
var deOldifyEngine = new DeOldify();  

Console.WriteLine("Initialize DeOldify engine...");
deOldifyEngine.Initialize(modelsDirectoryPath);
Console.WriteLine("Initialized.");

Console.WriteLine("");
var filesToConvert = Directory.GetFiles(exeDirectoryPath, @"TestFiles/pict*.*");
if (args.Length > 0 && Directory.Exists(args[0]))
{
    filesToConvert = Directory.GetFiles(args[0]);
    Console.WriteLine($"Colorize files in: {args[0]}");
}
else
{
    Console.WriteLine($"Colorize files in: {exeDirectoryPath}/TestFiles/");
}

var outputFilesPath = string.Empty;
if (args.Length > 1 && Directory.Exists(args[1]))
{
    outputFilesPath = args[1];
}

//################################################################################################################
// ### COLORIZE ###
//################################################################################################################
var stopWatch = new Stopwatch();
foreach (var fileName in filesToConvert)
{
    var fileInfo = new FileInfo(fileName);
    var fileNameWithoutExt = String.Concat(fileInfo.Name.Take(fileInfo.Name.Length - fileInfo.Extension.Length));
    var newFileName = $"{fileNameWithoutExt}-color{fileInfo.Extension}";
    
    if (string.IsNullOrEmpty(outputFilesPath))
    {
        outputFilesPath = fileInfo.DirectoryName;
    }
    
    var colorFile = Path.Combine(outputFilesPath, newFileName);
    
    Console.WriteLine($"Start colorize:\t{DateTime.Now.TimeOfDay:hh\\:mm\\:ss\\:fff} \t\t\t\t\t {fileInfo.Name}");
    
    stopWatch.Reset();
    stopWatch.Start();
    deOldifyEngine.Colorize(fileName, colorFile);
    stopWatch.Stop();
    
    Console.WriteLine(
        $"End colorize: \t{DateTime.Now.TimeOfDay:hh\\:mm\\:ss\\:fff} \t Elapsed time:\t{stopWatch.Elapsed:hh\\:mm\\:ss\\:fff} \t {newFileName}");
    Console.WriteLine("");
}
//################################################################################################################
// ### COLORIZE ###
//################################################################################################################













