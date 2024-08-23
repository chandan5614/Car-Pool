namespace Core.Interfaces
{

    public interface IBlobService
    {
        Task<string> UploadDocumentAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadDocumentAsync(string fileName);
    }
}

