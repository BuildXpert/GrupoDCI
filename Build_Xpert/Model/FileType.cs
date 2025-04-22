namespace Build_Xpert.Model
{
    public class FileType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }

        public bool AllowedForProject { get; set; }
        public bool AllowedForProfilePicture { get; set; }
    }
}
