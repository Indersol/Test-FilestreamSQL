namespace Test_FilestreamSQL.Models.Support
{
    public class FileStreamRowData
    {
        public required string Path { get; set; }
        public required byte[] Transaction { get; set; }
    }
}
