using SixLabors.ImageSharp;

namespace Viberz.Application.Utilities;

public class ConvertImageToBase64
{
    private readonly HttpClient _httpClient;

    public ConvertImageToBase64(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<byte[]> ConvertJpgUrlToWebp(string imageUrl)
    {
        byte[] imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);

        Image image = Image.Load(imageBytes);
        MemoryStream memoryStream = new MemoryStream();

        await image.SaveAsWebpAsync(memoryStream);

        return memoryStream.ToArray();
    }
}
