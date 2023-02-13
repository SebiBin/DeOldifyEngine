namespace DeOldifyEngine;

using System.Reflection;
using SkiaSharp;

public partial class DeOldify
{
    public bool CheckModels(string modelName)
    {
        using var br = GetModelsDataBinaryReader(true, false);
        var istEmpty = br.BaseStream.Length > 0;

        br.Close();

        return istEmpty;
    }

    public bool Colorize(string inputPath, string outputPath)
    {
        //var inputBitmap = new BitmapTbl(inputPath);
        //using var fs = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
        //var skBitmap = SKBitmapEx.Decode(fs);
        var inputBitmap = new SkBitmapEx();
        inputBitmap.Open(inputPath);
        
        var result = Colorize(inputBitmap);

        var fileInfo = new FileInfo(outputPath);
        var format = fileInfo.Extension.ToLower() switch
        {
            ".jpg" => SKEncodedImageFormat.Jpeg,
            ".jpeg" => SKEncodedImageFormat.Jpeg,
            ".png" => SKEncodedImageFormat.Png,
            ".webp" => SKEncodedImageFormat.Webp,
            ".bmp" => SKEncodedImageFormat.Bmp,
            _ => SKEncodedImageFormat.Bmp
        };

        result.Save(outputPath, format);

        return true;
    }
}