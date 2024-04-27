
public class Program
{
    public static void Main(string[] args)
    {
        var commandHandler = new CommandHandler();
        commandHandler.Handle(args).Wait();

        //var store = new MinioFileStoreKiuss("localhost:9000", "ROOTUSER", "CHANGEME123");
        //var content = File.ReadAllBytes("F:\\dp_emulation\\4ced9581-a08b-433b-b1a6-c99e1e393684\\2e455c4b-cfc7-4b68-be8e-9a9013e91644.txt");
        //store.CreateFile(new Guid("2e455c4b-cfc7-4b68-be8e-9a9013e91644"), new Guid("4ced9581-a08b-433b-b1a6-c99e1e393684"), content, "2e455c4b-cfc7-4b68-be8e-9a9013e91644.txt");
        //store.DeleteFile(new Guid("2e455c4b-cfc7-4b68-be8e-9a9013e91644"), new Guid("4ced9581-a08b-433b-b1a6-c99e1e393684"),"2e455c4b-cfc7-4b68-be8e-9a9013e91644.txt");
    }
}
