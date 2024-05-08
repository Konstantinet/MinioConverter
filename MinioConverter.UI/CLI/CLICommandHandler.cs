using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.CommandLine;
using Microsoft.Extensions.Configuration;
using MinioConverter.Domain.Domain;
using MinioConverter.Domain.Data;
using Dt.Kpsirs.Common.File;

namespace MinioConverter.UI.CLI
{
    internal class CLICommandHandler:UIHandler
    {
        static string endpoint;
        static string login;
        static string password;
        public async Task<int> Handle(IConfiguration config,params string[] args)
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
            var filePath = new Option<string>(
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
               description: "Connection link");
            var userName = new Argument<string>
           (
               name: "login",
               description: "Username");
            var userPassword = new Argument<string>
           (
               name: "password",
               description: "User password");

            var rootCommand = new RootCommand("Service for migrating drilling projects to Minio storage");
            rootCommand.AddArgument(connectionLink);
            rootCommand.AddArgument(userName);
            rootCommand.AddArgument(userPassword);
            rootCommand.SetHandler(() =>
            {
               

            });

            var connectCommand = new Command("connect", "Enter Minio connection data");
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
                project
            };
            rootCommand.AddCommand(sendCommand);
            sendCommand.SetHandler<string,string?>(async (projectType,project) =>
            {
                
                if (projectType == "kiuss")
                {
                    var storage = new MinioFileStoreKiuss(config["ConnectionArgs:endpoint"],
                        config["ConnectionArgs:login"],
                        config["ConnectionArgs:password"]);
                    if (project is not null)
                    {
                        DirectoryInfo d = new DirectoryInfo(project); //Assuming Test is your Folder

                        FileInfo[] dirs = d.GetFiles("*.*");
                        foreach(var file in dirs)
                        {
                            var fileGuid = file.FullName.Split("\\").Last().Split('.')[0];
                            await storage.CreateFile(new Guid(fileGuid),
                                new Guid(project.Split("\\").Last()),
                                new FileExtractor(file.FullName).GetFile(),
                                file.FullName);
                        }
                    }
                }
                if(projectType == "kpsirs")
                {
                    if (project is not null)
                    {
                        var storage = new MinioFileStoreKpsirs(config["ConnectionArgs:endpoint"],
                        config["ConnectionArgs:login"],
                        config["ConnectionArgs:password"]);
                        DirectoryInfo projectDir = new DirectoryInfo(project); //Assuming Test is your Folder
                        foreach (var d in projectDir.GetDirectories())
                        {
                            FileInfo[] dirs = d.GetFiles("*.*");
                            foreach (var file in dirs)
                            {
                                var fileGuid = file.FullName.Split("\\").Last().Split('.')[0];
                                await storage.CreateFile(new Guid(fileGuid),
                                    new Guid(project.Split("\\").Last()),
                                    new FileExtractor(file.FullName).GetFile(),
                                    file.FullName,
                                    GetFileType(d.Name));
                            }
                        }
                    }
                }

            }, projectType, project);
            



            var commandLineBuilder = new CommandLineBuilder(rootCommand)
                .UseDefaults();
            var parser =commandLineBuilder.Build();
            return  await parser.InvokeAsync(args).ConfigureAwait(false);    

        }
        
        private FileType GetFileType(string fileType)
        {
            if(fileType == "Rdrill")
            {
                return FileType.Rdrill;
            }
            if (fileType == "Attachment")
            {
                return FileType.Attachment;
            }
            if (fileType == "Report")
            {
                return FileType.Report;
            }
           else
            {
                return FileType.Image;
            }
            
        }
    }
}
