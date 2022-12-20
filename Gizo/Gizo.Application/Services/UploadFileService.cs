using Gizo.Application.Options;
using Gizo.Application.Trips.Dtos;
using Gizo.Utility;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Services;

public class UploadFileService
{
    private readonly UploadFileSettings _uploadFileSettings;
    private readonly int _chunkSize;

    public UploadFileService(IOptions<UploadFileSettings> uploadFileSettings)
    {
        _uploadFileSettings = uploadFileSettings.Value;
        _chunkSize = FileHelper.MBToByte(_uploadFileSettings.ChunkSize);
    }

    public TripCreatedFileDto CreateUserFile(string webRootPath)
    {
        var currentDateTime = DateTime.Now.ToStandardDateTime();
        var currentDate = DateTime.Now.ToStandardDate();

        var filePath = Path.Combine(
            webRootPath,
            string.Format(_uploadFileSettings.SaveTo, currentDate, currentDateTime));

        var tempFilePath = Path.Combine(
            webRootPath,
            string.Format(_uploadFileSettings.SaveTempTo, currentDate, currentDateTime));

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            Directory.CreateDirectory(tempFilePath);
        }

        return new TripCreatedFileDto(filePath, _chunkSize);
    }

    public async Task UploadChunk(
        string fileName,
        string filePath,
        Stream fileChunk)
    {
        try
        {
            string newpath = Path.Combine(filePath + "/Temp", fileName);

            using FileStream fs = File.Create(newpath);
            byte[] bytes = new byte[_chunkSize];
            int bytesRead = 0;

            while ((bytesRead = await fileChunk.ReadAsync(bytes, 0, bytes.Length)) > 0)
            {
                fs.Write(bytes, 0, bytesRead);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public string UploadCompleted(
        string filePath,
        string fileName)
    {
        try
        {
            string tempPath = Path.Combine(filePath, "Temp");
            string newPath = Path.Combine(tempPath, fileName);

            string[] filePaths = Directory.GetFiles(tempPath)
                                    .Where(p => p.Contains(fileName))
                                    .OrderBy(p => Int32.Parse(p.Replace(fileName, "$")
                                    .Split('$')[1])).ToArray();

            foreach (string fp in filePaths)
            {
                MergeChunks(newPath, fp);
            }

            File.Move(
                Path.Combine(tempPath, fileName),
                Path.Combine(filePath, fileName));

            return fileName;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static void MergeChunks(
        string finalPath,
        string chunkPath)
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " : " + ex.StackTrace);
        }
        finally
        {
            if (resultFs != null) resultFs.Close();
            if (chunkFs != null) chunkFs.Close();
            File.Delete(chunkPath);
        }
    }
}
