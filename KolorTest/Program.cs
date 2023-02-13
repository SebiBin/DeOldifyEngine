using System.Diagnostics;
using DeOldifyEngine;
using System.Reflection;

Console.WriteLine("Test konwerji zdjęć");

var deOldifyEngine = new DeOldify();  

deOldifyEngine.Initialize();

var inputPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
Console.WriteLine($"Przetwarzam pliki w: {inputPath}");
Console.WriteLine("");
var filesToConvert = Directory.GetFiles(inputPath, @"TestFiles/*.jpg");

var stopWatch = new Stopwatch();


//var fileName = Path.Combine(inputPath, "TestFiles" , "pict1.jpg");
//var fileName = Path.Combine(inputPath, "TestFiles" , "pict2.webp");
//var fileName = Path.Combine(inputPath, "TestFiles" , "pict3.jpg");
var fileName = Path.Combine(inputPath, "TestFiles" , "pict4.jpg");


var fileInfo = new FileInfo(fileName);
var colorFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name}-color{fileInfo.Extension}");

Console.WriteLine($"Przetwarzam plik: {fileName}");

stopWatch.Start();
deOldifyEngine.Colorize(fileName, colorFile);
stopWatch.Stop();

Console.WriteLine($"Czas koloryzacji: {stopWatch.Elapsed}\t  => {colorFile}");



