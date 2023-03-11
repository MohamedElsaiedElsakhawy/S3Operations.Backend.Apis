using Microsoft.AspNetCore.Mvc;
using S3.BaseClass;
using S3.Contract;
using S3.Repository;
using S3.Request;
using System.Net.Mime;

namespace Backend.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3OperationController : ControllerBase
    {
        private IS3Storage _storage;
        public S3OperationController(IS3Storage storage)
        {
            _storage = storage;
        }

        [HttpPost("create-bucket")]
        public Task<ApiResponse<bool>> Post(string bucketName)
        {
            var response = _storage.CreateBucket(new CreateBucketRequest
            {
                bucketName = bucketName
            });

            return response;
        }

        [HttpGet("get-buckets")]
        public async Task<ApiResponse<IList<string>>> ListOfBuckets()
        {
            var response = await _storage.ListOfBucket();

            return response;
        }


        [HttpPost("create-folders")]
        public Task<ApiResponse<bool>> CreateFolder(string bucketName, string folderName)
        {
            var response = _storage.CreateFoldersAsync(new CreateFolderRequest
            {
                bucketName = bucketName,
                folderName = folderName
            });

            return response;
        }

        [HttpPost("create-files")]
        public Task<ApiResponse<FileBase>> Upload(string bucketName, string filePath, List<IFormFile> files)
        {
            var response = _storage.UploadFiles(new UploadFileRequest
            {
                bucketName = bucketName,
                filePath = filePath,
                files = files
            });

            return response;
        }

        [HttpGet("list-files")]
        public Task<ApiResponse<IList<string>>> List(string bucketName, string fullPath)
        {
            var response = _storage.ListAllFiles(new ListFilesRequest
            {
                bucketName = bucketName,
                fullPath = fullPath
            });

            return response;
        }

        [HttpGet("download-file")]
        public async Task<IActionResult> Download(string bucketName, string filePath, string fileName)
        {
            // folder1 
            // 1.txt
            var response = await _storage.DownloadFile(new DownloadFileRequest
            {
                bucketName = bucketName,
                fileName = fileName,
                filePath = filePath
            });

            return File(response.Item1, MediaTypeNames.Application.Octet, fileName);
            //return new Task<(Stream, string)>(x => (response.Result.Item1, response.Result.Item2),true);
        }

        [HttpDelete("delete-file")]
        public Task<ApiResponse<bool>> Delete(string bucketName, string filePath)
        {
            var response = _storage.DeleteFile(new DeleteFileRequest
            {
                bucketName = bucketName,
                filePath = filePath // folder1/1.txt
            });

            return response;
        }
    }
}
