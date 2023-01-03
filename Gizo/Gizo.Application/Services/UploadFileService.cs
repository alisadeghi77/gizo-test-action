using System.Globalization;
using CsvHelper;
using Gizo.Application.Options;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Contracts.Enums;
using Gizo.Domain.Exceptions;
using Gizo.Utility;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Services;

public class UploadFileService
{
    private readonly UploadFileSettings _uploadFileSetting;

    public UploadFileService(IOptions<UploadFileSettings> uploadFileSettings)
    {
        _uploadFileSetting = uploadFileSettings.Value;
    }

    public bool CheckVideoType(TripFileType tripFileType, string type)
    {
        return tripFileType switch
        {
            TripFileType.Video => _uploadFileSetting.VideoAcceptTypes.Contains(type.ToLower()),
            TripFileType.IMU => _uploadFileSetting.IMUAcceptTypes.Contains(type.ToLower()),
            TripFileType.GPS => _uploadFileSetting.GPSAcceptTypes.Contains(type.ToLower()),
            _ => false,
        };
    }

    public TripCreatedFileDto CreateUserFile(string filePath)
    {
        var chunkPath = Path.Combine(filePath, "Chunk");

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            Directory.CreateDirectory(chunkPath);
        }

        return new TripCreatedFileDto(filePath);
    }

    public async Task UploadChunk(string fileName, string filePath, Stream fileChunk, int chunkSize)
    {
        CreateUserFile(filePath);

        string newpath = Path.Combine(filePath + "/Chunk", fileName);

        using FileStream fs = File.Create(newpath);
        byte[] bytes = new byte[chunkSize];
        int bytesRead = 0;

        while ((bytesRead = await fileChunk.ReadAsync(bytes, 0, bytes.Length)) > 0)
        {
            fs.Write(bytes, 0, bytesRead);
        }
    }

    public string UploadCompleted(string filePath, string fileName, string type)
    {
        var newFileName = fileName + type.ToStandardType();
        string tempPath = Path.Combine(filePath, "Chunk");
        string newPath = Path.Combine(tempPath, newFileName);

        string[] filePaths = Directory.GetFiles(tempPath)
            .Where(p => p.Contains(newFileName))
            .OrderBy(p => p.Replace(newFileName, "$")
                .Split('$')[1]).ToArray();

        foreach (string fp in filePaths)
        {
            MergeChunks(newPath, fp);
        }

        File.Move(
            Path.Combine(tempPath, newFileName),
            Path.Combine(filePath, newFileName));

        return fileName;
    }

    public TripImuDateTimeDto? ImuStartAndEndDateTime(long userId, long tripId, string tempPath)
    {
        var path = GetTripTempFilePath(tempPath,
            userId, tripId, TripFileType.IMU.ToString());

        var directory = Directory.GetFiles(path);
        if (!directory.Any())
            throw new FileUploadedException("Directory is Empty");

        using var reader = new StreamReader(directory[0]);
        using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
        var imuRecords = csv.GetRecords<TripImuReadCsvDto>();

        if (imuRecords == null)
        {
            return null;
        }

        var tripStartDateTime = imuRecords.FirstOrDefault()?.Time.ToUniversalTime() ??
                                throw new FileUploadedException("trip Start Date Time is null");

        var tripEndDateTime = imuRecords.LastOrDefault()?.Time.ToUniversalTime() ??
                              throw new FileUploadedException("trip end Date Time is null");

        return new TripImuDateTimeDto(tripStartDateTime, tripEndDateTime);
    }

    public void MergeChunks(string finalPath, string chunkPath)
    {
        FileStream? resultFs = null;
        FileStream? chunkFs = null;
        try
        {
            resultFs = File.Open(finalPath, FileMode.Append);
            chunkFs = File.Open(chunkPath, FileMode.Open);
            byte[] resultContent = new byte[chunkFs.Length];
            chunkFs.Read(resultContent, 0, (int)chunkFs.Length);
            resultFs.Write(resultContent, 0, (int)chunkFs.Length);
        }
        catch
        {
            // LOG
        }
        finally
        {
            if (resultFs != null) resultFs.Close();
            if (chunkFs != null) chunkFs.Close();
            File.Delete(chunkPath);
        }
    }

    public string CreateTripFolder(string folderPath,
        long userId,
        string carModel,
        string carLicense,
        DateTime startDate)
    {
        var folder = string.Format(folderPath,
            userId,
            carModel,
            carLicense,
            startDate.ToStandardDate(),
            startDate.ToStandardDateTime());

        var path = Path.Combine(_uploadFileSetting.SaveTo, folder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return folder;
    }

    public void MoveFiles(string tempPath, string path, string desFileName, TripFileType tripFileType)
    {
        var directory = Directory.GetFiles(tempPath);

        if (!directory.Any())
            throw new FileUploadedException("Directory is Empty");

        var moveTo = Path.Combine(_uploadFileSetting.SaveTo,
            path,
            desFileName);

        File.Move(directory[0], moveTo);
    }

    public string GetTripTempFilePath(string tempPath, long userId, long tripId, string tripFileType)
    {
        return Path.Combine(tempPath, userId.ToString(), tripId.ToString(), tripFileType);
    }
}
