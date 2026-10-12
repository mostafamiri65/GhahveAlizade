namespace GhahveAlizade.Application.Common.Interfaces;

public record ImageDimensions(int Width, int Height);

public record ImageConstraints(
    int MinWidth,
    int MinHeight,
    double? AspectRatio = null,      // مثلاً 1.5 برای 3:2 یا 1.0 برای مربع
    double AspectTolerance = 0.02);  // ±2%

public interface IImageValidator
{
    /// <summary>ابعاد تصویر را از هدر فایل می‌خواند؛ null اگر فرمت خواندنی نباشد.</summary>
    Task<ImageDimensions?> GetDimensionsAsync(Stream stream, CancellationToken ct = default);

    /// <summary>بررسی تصویر در برابر محدودیت‌ها؛ پیام خطای فارسی برمی‌گرداند یا null.</summary>
    Task<string?> ValidateAsync(Stream stream, ImageConstraints constraints, CancellationToken ct = default);
}