using System;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(config.Value.CloudName,
        config.Value.ApiKey,
        config.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }
    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
        
            }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
    {
        var UploadResult = new ImageUploadResult();
        if (file != null)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams =new ImageUploadParams
            {
                File = new FileDescription(file.FileName,stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "da-ang20"
            };
            UploadResult= await _cloudinary.UploadAsync(uploadParams);
        }
        return UploadResult;
    }
}
