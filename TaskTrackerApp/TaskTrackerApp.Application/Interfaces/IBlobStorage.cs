namespace TaskTrackerApp.Application.Interfaces;

public interface IBlobStorage
{
    string GetPublicUrl(string fileName);
    Task UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);
    Task<bool> ExistsFileAsync(string fileName, CancellationToken ct = default);
}