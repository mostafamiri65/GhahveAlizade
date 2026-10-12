using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<LocalFileStorageService> _logger;
    private readonly IImageValidator _imageValidator;

    private static readonly string[] AllowedExtensions =
        { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg" };

    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public LocalFileStorageService(
        IHostEnvironment environment,
        ILogger<LocalFileStorageService> logger,
        IImageValidator imageValidator)
    {
        _environment = environment;
        _logger = logger;
        _imageValidator = imageValidator;
    }

    public async Task<FileUploadResult> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        string folder,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // اعتبارسنجی پسوند
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                return new FileUploadResult(false,
                    ErrorMessage: "فرمت فایل مجاز نیست. فقط تصاویر JPG، PNG، WebP، GIF، SVG مجاز هستند.");

            // اعتبارسنجی Content Type
            if (!contentType.StartsWith("image/"))
                return new FileUploadResult(false,
                    ErrorMessage: "فایل باید از نوع تصویر باشد.");

            // بررسی حجم
            if (stream.CanSeek && stream.Length > MaxFileSizeBytes)
                return new FileUploadResult(false,
                    ErrorMessage: "حجم فایل نمی‌تواند بیشتر از 5 مگابایت باشد.");

            // ساخت مسیر
            var now = DateTime.UtcNow;
            var safeFolder = folder.Trim('/').Replace("..", "").Replace("\\", "/");
            var relativeFolder = $"{safeFolder}/{now:yyyy}/{now:MM}";

            var uniqueName = $"{Guid.NewGuid():N}{extension}";
            var relativePath = $"{relativeFolder}/{uniqueName}";

            var webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            var physicalFolder = Path.Combine(webRootPath, "uploads", relativeFolder.Replace('/', Path.DirectorySeparatorChar));
            var physicalPath = Path.Combine(webRootPath, "uploads", relativePath.Replace('/', Path.DirectorySeparatorChar));

            // ساخت پوشه
            Directory.CreateDirectory(physicalFolder);

            // ذخیره فایل
            await using (var fileStream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream, cancellationToken);
            }

            var url = $"/uploads/{relativePath}";

            _logger.LogInformation("فایل آپلود شد: {Url}", url);

            return new FileUploadResult(
                Success: true,
                RelativePath: relativePath,
                Url: url,
                SizeBytes: new FileInfo(physicalPath).Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در آپلود فایل {FileName}", fileName);
            return new FileUploadResult(false, ErrorMessage: "خطا در آپلود فایل.");
        }
    }

    public async Task<FileUploadResult> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        string folder,
        ImageConstraints? constraints,
        CancellationToken cancellationToken = default)
    {
        if (constraints is not null)
        {
            var err = await _imageValidator.ValidateAsync(stream, constraints, cancellationToken);
            if (err is not null)
            {
                _logger.LogWarning("آپلود تصویر رد شد: {Reason}", err);
                return new FileUploadResult(false, ErrorMessage: err);
            }
        }
        return await UploadAsync(stream, fileName, contentType, folder, cancellationToken);
    }

    public Task<bool> DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return Task.FromResult(false);

            var webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            var physicalPath = Path.Combine(webRootPath, "uploads",
                relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در حذف فایل {Path}", relativePath);
            return Task.FromResult(false);
        }
    }

    public Task<bool> ExistsAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return Task.FromResult(false);

        var webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        var physicalPath = Path.Combine(webRootPath, "uploads",
            relativePath.Replace('/', Path.DirectorySeparatorChar));

        return Task.FromResult(File.Exists(physicalPath));
    }
}