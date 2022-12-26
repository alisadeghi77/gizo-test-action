using Gizo.Application.Trips.Dtos;
using Gizo.Utility;

namespace Gizo.Application.Services;

public class UploadFileService
{
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
        try
        {
            var filePathResult = CreateUserFile(filePath);

            string newpath = Path.Combine(filePath + "/Chunk", fileName);

            using FileStream fs = File.Create(newpath);
            byte[] bytes = new byte[chunkSize];
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

    public string UploadCompleted(string filePath, string fileName, string type)
    {
        try
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

    public string GetTripTempFilePath(string tempPath, long userId, long tripId, string tripFileType)
    {
        return Path.Combine(tempPath, userId.ToString(), tripId.ToString(), tripFileType);
    }
}
