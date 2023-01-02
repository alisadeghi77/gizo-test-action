namespace Gizo.Application.Options;

public class UploadFileSettings
{
    public int ChunkSize { get; set; }

    public string SaveTo { get; set; }

    public string SaveTempTo { get; set; }

    public string FolderStructure { get; set; }

    public List<string> VideoAcceptTypes { get; set; }

    public List<string> IMUAcceptTypes { get; set; }

    public List<string> GPSAcceptTypes { get; set; }
}
