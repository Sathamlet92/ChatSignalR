namespace BlazingChat.Shared.Models.DTOs;
public class ImageDto
{
    public string FolderName { get; set; } = null!;
    public string ImageName { get; set; } = null!;
    public byte[] ImageData { get; set; } = null!;
}