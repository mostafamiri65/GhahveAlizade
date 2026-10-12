namespace GhahveAlizade.Application.Common.Interfaces;

public interface IFileStorageService
{
	Task<FileUploadResult> UploadAsync(
		Stream stream,
		string fileName,
		string contentType,
		string folder,
		CancellationToken cancellationToken = default);

	Task<bool> DeleteAsync(string relativePath, CancellationToken cancellationToken = default);

	Task<bool> ExistsAsync(string relativePath, CancellationToken cancellationToken = default);
}

public record FileUploadResult(
	bool Success,
	string? RelativePath = null,
	string? Url = null,
	long? SizeBytes = null,
	string? ErrorMessage = null);