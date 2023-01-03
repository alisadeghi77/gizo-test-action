using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enums;

namespace Gizo.Domain.Aggregates.TripAggregate;

public class TripTempFile : BaseEntity<long>, ICreateDate
{
    internal TripTempFile(long tripId,
        string fileName,
        string chunkId,
        string fileType,
        TripFileType tripFileType)
    {
        TripId = tripId;
        FileName = fileName;
        CreateDate = DateTime.UtcNow;
        ChunkId = chunkId;
        FileType = fileType;
        TripFileType = tripFileType;
    }

    public long TripId { get; private set; }

    public string FileName { get; private set; }

    public string FileType { get; private set; }

    public string ChunkId { get; private set; }

    public TripFileType TripFileType { get; private set; }

    public DateTime CreateDate { get; private set; }

    public Trip Trip { get; private set; } = null!;
}
