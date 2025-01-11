namespace LibraryManagementSystem.Extensions
{
    public static class FileExtensions
    {
        public static byte[] ToByteArray(this IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
