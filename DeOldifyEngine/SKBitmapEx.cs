namespace DeOldifyEngine;

using System.Runtime.InteropServices;
using SkiaSharp;

public class SkBitmapEx : SKBitmap
{
    public SkBitmapEx() : base()
    {
    }

    public SkBitmapEx(SKBitmap source, int width, int height, bool isOpaque = false) : base(width, height, isOpaque)
    {
        source.ScalePixels(this, SKFilterQuality.High);
    }
    
    public SkBitmapEx(int width, int height, bool isOpaque = false) : base(width, height, isOpaque)
    {
    }

    public SkBitmapEx(int width, int height, SKColorType colorType, SKAlphaType alphaType) : base(width, height, colorType, alphaType)
    {
    }

    public SkBitmapEx(
        int width,
        int height,
        SKColorType colorType,
        SKAlphaType alphaType,
        SKColorSpace colorspace)
        : base(width, height, colorType, alphaType, colorspace)
    {
    }

    public SkBitmapEx(SKImageInfo info) : base(info)
    {
    }

    public SkBitmapEx(SKImageInfo info, int rowBytes) : base(info, rowBytes)
    {
    }
        
    // public new System.Drawing.Color GetPixel(int x, int y)
    // {
    //     var skColor = base.GetPixel(x, y);
    //
    //     return System.Drawing.Color.FromArgb(skColor.Alpha, skColor.Red, skColor.Green, skColor.Blue);
    // }

    public SkBitmapEx Open(string fileName)
    {
        using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        var opened = SKBitmap.Decode(fs);
        
        GCHandle gcHandle = GCHandle.Alloc(opened.Bytes, GCHandleType.Pinned);
        //SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        
        IntPtr ptr = gcHandle.AddrOfPinnedObject();
        int rowBytes = opened.Info.RowBytes;
        this.InstallPixels(opened.Info, ptr, rowBytes, delegate { gcHandle.Free(); });
        
        return this;
    }
    
    public void Save(string fileName, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        using FileStream fs = new(fileName, FileMode.Create);
        
        this.Encode(fs, format, quality);
    }
}