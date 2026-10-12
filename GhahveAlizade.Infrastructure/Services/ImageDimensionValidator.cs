using GhahveAlizade.Application.Common.Interfaces;

namespace GhahveAlizade.Infrastructure.Services;

public class ImageDimensionValidator : IImageValidator
{
    public async Task<ImageDimensions?> GetDimensionsAsync(Stream stream, CancellationToken ct = default)
    {
        if (!stream.CanSeek) return null;
        var startPosition = stream.Position;
        try
        {
            var header = new byte[64 * 1024];
            var read = await stream.ReadAsync(header.AsMemory(0, header.Length), ct);
            if (read < 24) return null;
            stream.Position = startPosition;

            // PNG: signature 8B + IHDR → width @16, height @20 (big-endian)
            if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
            {
                int w = (header[16] << 24) | (header[17] << 16) | (header[18] << 8) | header[19];
                int h = (header[20] << 24) | (header[21] << 16) | (header[22] << 8) | header[23];
                return (w > 0 && h > 0) ? new ImageDimensions(w, h) : null;
            }

            // JPEG: FFD8 start، سپس اسکن markerهای SOF
            if (header[0] == 0xFF && header[1] == 0xD8)
                return ScanJpeg(header, read);

            return null;
        }
        finally
        {
            stream.Position = startPosition;
        }
    }

    private static ImageDimensions? ScanJpeg(byte[] b, int length)
    {
        int i = 2;
        while (i + 9 < length)
        {
            if (b[i] != 0xFF) { i++; continue; }
            var marker = b[i + 1];
            if (marker == 0xD8 || marker == 0x01 || (marker >= 0xD0 && marker <= 0xD7)) { i += 2; continue; }
            if (i + 3 >= length) break;
            int segLen = (b[i + 2] << 8) | b[i + 3];
            // SOF0..SOF15 به‌جز DHT(C4)، JPG(C8)، DAC(CC)
            if (marker is >= 0xC0 and <= 0xCF && marker != 0xC4 && marker != 0xC8 && marker != 0xCC)
            {
                if (i + 8 < length)
                {
                    int h = (b[i + 5] << 8) | b[i + 6];
                    int w = (b[i + 7] << 8) | b[i + 8];
                    return (w > 0 && h > 0) ? new ImageDimensions(w, h) : null;
                }
                break;
            }
            i += 2 + segLen;
        }
        return null;
    }

    public async Task<string?> ValidateAsync(Stream stream, ImageConstraints c, CancellationToken ct = default)
    {
        var d = await GetDimensionsAsync(stream, ct);
        if (d is null)
            return "ابعاد تصویر قابل تشخیص نیست. فقط PNG و JPEG پشتیبانی می‌شود.";

        if (d.Width < c.MinWidth || d.Height < c.MinHeight)
            return $"ابعاد تصویر باید حداقل {c.MinWidth}×{c.MinHeight} پیکسل باشد (تصویر ارسالی: {d.Width}×{d.Height}).";

        if (c.AspectRatio is { } target)
        {
            var actual = (double)d.Width / d.Height;
            if (Math.Abs(actual - target) / target > c.AspectTolerance)
                return $"نسبت ابعاد تصویر باید {RatioLabel(target)} باشد (تصویر ارسالی: {d.Width}×{d.Height}).";
        }
        return null;
    }

    private static string RatioLabel(double r) =>
        Math.Abs(r - 1.0) < 0.01 ? "مربع (1:1)" :
        Math.Abs(r - 1.5) < 0.01 ? "3:2 (افقی)" : r.ToString("0.##");
}