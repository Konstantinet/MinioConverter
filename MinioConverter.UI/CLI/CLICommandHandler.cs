using Dt.Kiuss.Supervisor.Domain.Utils.File;
using MinioConverter.UI;
using System.CommandLine;
using System.IO;

namespace MinioConverter.CLI
{
    internal class CommandHandler:UIHandler
    {
        static string endpoint;
        static string login;
        static string password;
        public async Task Handle(params string[] args)
        {
            var projectType = new Option<string>(
            aliases: new[] { "-p", "--project-type" },
            description: "Project type kiuss / kpsirs").FromAmong(
            "kiuss",
            "kpsirs")
            ; 
            var project = new Option<string>(
            aliases: new[] { "-r" },
            description: "Project GUID")
            {
                IsRequired = false,
                Arity = ArgumentArity.ExactlyOne
            };
            var file = new Option<string>(
            aliases: new[] { "-f", },
            description: "File Name")
            {
                IsRequired = false,
                Arity = ArgumentArity.ExactlyOne
            };
            var pathArgument = new Argument<string>
            ( 
                name:"path", 
                description: "Path to file saving directory");

            var objectNameArgument = new Argument<string>
            (
                name:"ObjectName", 
                description:"Name of minio object");
            var connectionLink = new Argument<string>
           (
               name: "link",
               description: "Name of minio object");
            var userName = new Argument<string>
           (
               name: "login",
               description: "Name of minio object");
            var userPassword = new Argument<string>
           (
               name: "password",
               description: "Name of minio object");

            var rootCommand = new RootCommand("Sample app for System.CommandLine");
            rootCommand.AddArgument(connectionLink);
            rootCommand.AddArgument(userName);
            rootCommand.AddArgument(userPassword);
            rootCommand.SetHandler<string,string,string>((connectionLink,userName,userPassword) =>
            {
                endpoint = connectionLink;
                login = userName;
                password = userPassword;

            }, connectionLink, userName, userPassword);

            var connectCommand = new Command("connect", "Read and display the file.")
            {
            
            };
            rootCommand.AddArgument(connectionLink);
            rootCommand.AddArgument(userName);
            rootCommand.AddArgument(userPassword);
            rootCommand.AddCommand(connectCommand);
            connectCommand.SetHandler<string, string, string>((connectionLink, userName, userPassword) =>
            {
                endpoint = connectionLink;
                login = userName;
                password = userPassword;

            }, connectionLink, userName, userPassword);

            var sendCommand = new Command("send", "Send file or project folder to Mimio storage")
            {
                projectType,
                project,
                file
            };
            rootCommand.AddCommand(connectCommand);
            connectCommand.SetHandler<string,string?,string?>(async (projectType,project,file) =>
            {
                if (projectType == "kiuss")
                {
                    var storage = new MinioFileStoreKiuss(endpoint, login, password);   
                    if(project is not null)
                    {
                        DirectoryInfo d = new DirectoryInfo(@"F:\\dp_emulation\\kiuss\\"); //Assuming Test is your Folder

                        DirectoryInfo[] dirs = d.GetDirectories();
                        foreach(DirectoryInfo dir in dirs)
                        {

                        }
                        storage.CreateFile()
                    }
                }

            }, projectType, project, file);



        }
    }
}
