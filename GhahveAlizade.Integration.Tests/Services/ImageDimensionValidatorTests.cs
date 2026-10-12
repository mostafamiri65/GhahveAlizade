using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Infrastructure.Services;
using Xunit;

namespace GhahveAlizade.Integration.Tests.Services;

public class ImageDimensionValidatorTests
{
    private readonly ImageDimensionValidator _sut = new();

    private static byte[] PngBytes(int w, int h)
    {
        var b = new byte[33];
        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }.CopyTo(b, 0);
        // width @16, height @20 (big-endian)
        b[16] = (byte)(w >> 24); b[17] = (byte)(w >> 16); b[18] = (byte)(w >> 8); b[19] = (byte)w;
        b[20] = (byte)(h >> 24); b[21] = (byte)(h >> 16); b[22] = (byte)(h >> 8); b[23] = (byte)h;
        return b;
    }

    [Fact]
    public async Task GetDimensions_Valid_Png_Returns_Dimensions()
    {
        var d = await _sut.GetDimensionsAsync(new MemoryStream(PngBytes(1200, 800)));
        Assert.NotNull(d);
        Assert.Equal(1200, d.Width);
        Assert.Equal(800, d.Height);
    }

    [Fact]
    public async Task Validate_Within_Constraints_Passes()
    {
        var result = await _sut.ValidateAsync(
            new MemoryStream(PngBytes(1200, 800)),
            new ImageConstraints(1200, 800, 1.5));
        Assert.Null(result);
    }

    [Fact]
    public async Task Validate_Too_Small_Fails_With_Message()
    {
        var result = await _sut.ValidateAsync(
            new MemoryStream(PngBytes(600, 400)),
            new ImageConstraints(1200, 800, 1.5));
        Assert.NotNull(result);
        Assert.Contains("1200", result);
    }

    [Fact]
    public async Task Validate_Wrong_Ratio_Fails()
    {
        var result = await _sut.ValidateAsync(
            new MemoryStream(PngBytes(1200, 1200)),
            new ImageConstraints(1200, 800, 1.5));
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Validate_Unrecognized_Format_Fails()
    {
        var result = await _sut.ValidateAsync(
            new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                17, 18, 19, 20, 21, 22, 23, 24 }),
            new ImageConstraints(100, 100));
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Validate_Within_Tolerance_Passes()
    {
        // 1203×802 → نسبت ~1.499 که در تلورانس ±2% از 1.5 قرار می‌گیرد
        var result = await _sut.ValidateAsync(
            new MemoryStream(PngBytes(1203, 802)),
            new ImageConstraints(1200, 800, 1.5));
        Assert.Null(result);
    }
}