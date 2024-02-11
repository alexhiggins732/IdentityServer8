using System;
using System.IO;
using System.Linq;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace build
{
    partial class Program
    {
        private const string packOutput = "./artifacts";
        private const string packOutputCopy = "../../nuget";
        private const string envVarMissing = " environment variable is missing. Aborting.";

        private static class Targets
        {
            public const string CleanBuildOutput = "clean-build-output";
            public const string CleanPackOutput = "clean-pack-output";
            public const string Build = "build";
            public const string Test = "test";
            public const string Pack = "pack";
            public const string SignBinary = "sign-binary";
            public const string SignPackage = "sign-package";
            public const string CopyPackOutput = "copy-pack-output";
        }

        static void Main(string[] args)
        {
            Target(Targets.CleanBuildOutput, () =>
            {
                //Run("dotnet", "clean -c Release -v m --nologo", echoPrefix: Prefix);
            });

            Target(Targets.Build, DependsOn(Targets.CleanBuildOutput), () =>
            {
                Run("dotnet", "build -c Release --nologo", echoPrefix: Prefix);
            });

            Target(Targets.SignBinary, DependsOn(Targets.Build), () =>
            {
                Sign("./src/bin/Release", "*.dll");
            });

            Target(Targets.Test, DependsOn(Targets.Build), () =>
            {
                Run("dotnet", $"test -c Release --no-build", echoPrefix: Prefix);
            });

            Target(Targets.CleanPackOutput, () =>
            {
                if (Directory.Exists(packOutput))
                {
                    Directory.Delete(packOutput, true);
                }
            });

            Target(Targets.Pack, DependsOn(Targets.Build, Targets.CleanPackOutput), () =>
            {
                var project = Directory.GetFiles("./src", "*.csproj", SearchOption.TopDirectoryOnly).OrderBy(_ => _).First();

                Run("dotnet", $"pack {project} -c Release -o \"{Directory.CreateDirectory(packOutput).FullName}\" --no-build --nologo", echoPrefix: Prefix);
            });

            Target(Targets.SignPackage, DependsOn(Targets.Pack), () =>
            {
                Sign(packOutput, "*.nupkg");
            });

            Target(Targets.CopyPackOutput, DependsOn(Targets.Pack), () =>
            {
                Directory.CreateDirectory(packOutputCopy);

                foreach (var file in Directory.GetFiles(packOutput))
                {
                    File.Copy(file, Path.Combine(packOutputCopy, Path.GetFileName(file)), true);
                }
            });

            Target("quick", DependsOn(Targets.CopyPackOutput));

            Target("default", DependsOn(Targets.Test, Targets.CopyPackOutput));

            Target("sign", DependsOn(Targets.SignBinary, Targets.Test, Targets.SignPackage, Targets.CopyPackOutput));

            RunTargetsAndExit(args, ex => ex is SimpleExec.NonZeroExitCodeException || ex.Message.EndsWith(envVarMissing), Prefix);
        }

        private static void Sign(string path, string searchTerm)
        {
            var signClientSecret = Environment.GetEnvironmentVariable("SignClientSecret");

            if (string.IsNullOrWhiteSpace(signClientSecret))
            {
                throw new Exception($"SignClientSecret{envVarMissing}");
            }

            foreach (var file in Directory.GetFiles(path, searchTerm, SearchOption.AllDirectories))
            {
                Console.WriteLine($"  Signing {file}");
                Run("dotnet", $"SignClient sign -c ../../signClient.json -i {file} -r sc-ids@dotnetfoundation.org -s \"{signClientSecret}\" -n 'IdentityServer8'", noEcho: true);
            }
        }
    }
}
