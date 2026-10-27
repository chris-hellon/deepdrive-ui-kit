namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Pluggable image upload sink for rich-text / media components. The DeepDrive product
/// backs this with Azure Blob Storage; the kit ships no implementation so consumers can
/// wire their own (local disk, S3, a CDN, …). Register one in DI to enable uploads.
/// </summary>
public interface IImageUploader
{
    /// <summary>
    /// Persists the given image bytes and returns a public URL to the stored file.
    /// </summary>
    /// <param name="content">Raw image bytes.</param>
    /// <param name="fileName">Original file name (used for extension / content-type hints).</param>
    /// <param name="contentType">MIME type, e.g. <c>image/png</c>.</param>
    Task<string> UploadImageAsync(byte[] content, string fileName, string contentType, CancellationToken cancellationToken = default);
}
