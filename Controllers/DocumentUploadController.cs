using Microsoft.AspNetCore.Mvc;
using PropertyHub.Data;

namespace PropertyHub.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentUploadController : ControllerBase
{
    private const long MaxUploadBytes = 10 * 1024 * 1024;
    private static readonly string[] AllowedExtensions = new[] { ".pdf", ".png", ".jpg", ".jpeg", ".doc", ".docx" };

    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DocumentUploadController> _logger;

    public DocumentUploadController(IWebHostEnvironment environment, ILogger<DocumentUploadController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(MaxUploadBytes)]
    [IgnoreAntiforgeryToken]
    public async Task<ActionResult<DocumentUploadResult>> Upload([FromForm] IFormFile file, [FromForm] string? folder)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (file.Length > MaxUploadBytes)
        {
            return BadRequest($"File exceeds {MaxUploadBytes / (1024 * 1024)} MB limit.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (!IsAllowedExtension(extension))
        {
            return BadRequest("File type not allowed.");
        }

        var webRoot = string.IsNullOrWhiteSpace(_environment.WebRootPath)
            ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
            : _environment.WebRootPath;

        var uploadsRoot = Path.Combine(webRoot, "uploads");
        var safeFolder = SanitizeSegment(folder);
        if (!string.IsNullOrWhiteSpace(safeFolder))
        {
            uploadsRoot = Path.Combine(uploadsRoot, safeFolder);
        }

        Directory.CreateDirectory(uploadsRoot);

        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsRoot, storedFileName);

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        var relativeUrl = string.IsNullOrWhiteSpace(safeFolder)
            ? $"/uploads/{storedFileName}"
            : $"/uploads/{safeFolder}/{storedFileName}";

        var result = new DocumentUploadResult(
            file.FileName,
            storedFileName,
            file.ContentType ?? "application/octet-stream",
            file.Length,
            relativeUrl,
            safeFolder);

        _logger.LogInformation("Uploaded document {OriginalFileName} to {Url}", file.FileName, relativeUrl);

        return Created(relativeUrl, result);
    }

    private static bool IsAllowedExtension(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return false;
        }

        return AllowedExtensions.Any(allowed =>
            string.Equals(allowed, extension, StringComparison.OrdinalIgnoreCase));
    }

    private static string? SanitizeSegment(string? segment)
    {
        if (string.IsNullOrWhiteSpace(segment))
        {
            return null;
        }

        var sanitized = new string(segment.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_').ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? null : sanitized;
    }
}
