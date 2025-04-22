namespace GameCenter.Utilities.Injectors.Models
{
    public class InjectorSetting
    {
        public DatabaseSetting Database { get; set; }
        public EmailSetting Email { get; set; }
        public EncryptorSetting Encryptor { get; set; }
        public EncryptorSetting Token { get; set; }
        public ProjectSetting Authentication { get; set; }
        public FileManipulatorSetting FileManipulator { get; set; }
        public InjectorSetting()
        {
            Database = new DatabaseSetting();
            Email = new EmailSetting();
            Encryptor = new EncryptorSetting();
            Token = new EncryptorSetting();
            FileManipulator = new FileManipulatorSetting();
            Authentication = new ProjectSetting();
        }
    }
    public class ProjectSetting
    {
        public string Url { get; set; }
    }
    public class DatabaseSetting
    {
        public string DatabaseVersion { get; set; }
        public string ConnectionString { get; set; }
    }

    public class EmailSetting
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Credencial { get; set; }
    }
    public class FileManipulatorSetting
    {
        public BlobStorageSetting BlobStorage { get; set; }
        public BlobStorageSetting Queue { get; set; }
    }
    public class BlobStorageSetting
    {
        public string ConnectionString { get; set; }
    }
    public class EventGridSetting
    {
        public string Endpoint { get; set; }
        public string Key { get; set; }
    }
    public class EncryptorSetting
    {
        public string Key { get; set; }
    }

}
