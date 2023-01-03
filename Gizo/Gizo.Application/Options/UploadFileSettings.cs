namespace Gizo.Application.Options;

public class UploadFileSettings
{
    public int ChunkSize { get; set; }

    public string SaveTo { get; set; } = null!;

    public string SaveTempTo { get; set; } = null!;

    public string FolderStructure { get; set; } = null!;

    public List<string> VideoAcceptTypes { get; set; } = null!;

    public List<string> IMUAcceptTypes { get; set; } = null!;

    public List<string> GPSAcceptTypes { get; set; } = null!;
}
