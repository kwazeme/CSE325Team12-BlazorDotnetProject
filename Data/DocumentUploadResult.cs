namespace PropertyHub.Data;

public sealed record DocumentUploadResult(
    string OriginalFileName,
    string StoredFileName,
    string ContentType,
    long Size,
    string Url,
    string? Folder);
