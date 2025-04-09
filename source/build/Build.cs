using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.TeamCity;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Cleanup);
    
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [Parameter] readonly string Branch = "master";

    [Solution(GenerateProjects = true)] readonly Solution Solution;
    string FullVersion = "1.0.0.0";

    readonly AbsolutePath GlobalAssemblyInfoFile = RootDirectory / "GlobalAssemblyInfo.cs";
    string Version = "1.0.0.0";

    AbsolutePath SourceDirectory => RootDirectory / "source";
    AbsolutePath OutputDirectory => RootDirectory / ".build";
    AbsolutePath TestResultsDirectory => OutputDirectory / "testResults";
    AbsolutePath TestCoverageDirectory => OutputDirectory / "testCoverage";
    AbsolutePath TempOutputDirectory => OutputDirectory / "_temp";
    AbsolutePath ApplicationPackagesDirectory => OutputDirectory / "Packages" / "Applications";

    Project ProjectApi => Solution.Source.Api.DiscordClone_Api;
    Project ProjectMvc => Solution.Source.Client.DiscordClone_Mvc;
    Project ProjectDbMigrator => Solution.Source.Api.DiscordClone_DbMigrator;

    Project BuildService => Solution.Build._build;

    List<Project> DotnetProjects => new()
    {
        ProjectApi,
        ProjectMvc,
        ProjectDbMigrator
    };

    Target Init => _ => _
        .Executes(() =>
        {
            Log.Information("Reading assembly version from GlobalAssemblyInfo.cs");

            var assemblyInfo = GlobalAssemblyInfoFile.ReadAllLines();
            var versionInfoLine =
                assemblyInfo.FirstOrDefault(t => t.Contains("[assembly: AssemblyVersion") && !t.StartsWith("/"));
            if (versionInfoLine == null)
                throw new Exception("GlobalAssemblyInfo.cs does not contain a line defining the AssemblyVersion!");
            Version = versionInfoLine.Substring(versionInfoLine.IndexOf('(') + 2,
                versionInfoLine.LastIndexOf(')') - versionInfoLine.IndexOf('(') - 3);
            if (Branch.ToLower().Contains("feature"))
                FullVersion = string.Concat(Version, "-", "feature").Trim('-');
            else
                FullVersion = string.Concat(Version, "-", Branch.Replace('/', '-')).Trim('-');

            Log.Information("The version is: {Version}", Version);
            Log.Information("The full version is: {FullVersion}", FullVersion);

            Log.Information("Configuration is: {Configuration}", Configuration);
            Log.Information("Branch is: {Branch}", Branch);

            if (TeamCity.Instance != null)
                TeamCity.Instance.SetBuildNumber(Version);

            SourceDirectory.GlobDirectories("**/bin", "**/obj").WhereNot(d => d.Parent == BuildService.Directory)
                .ForEach(x => x.DeleteDirectory());
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Init)
        .Executes(() =>
        {
            foreach (var project in DotnetProjects)
                DotNetRestore(s => s
                        .SetProjectFile(project)
                    //.SetRuntime("win-x64")
                );
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            foreach (var project in DotnetProjects)
                DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .SetVersion(FullVersion)
                    .EnableNoRestore());
        });

    Target Tests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            // Tests
        });

    Target GenerateReport => _ => _
        .DependsOn(Tests)
        .Executes(() =>
        {
            // Generate tests about coverage
        });

    Target Publish => _ => _
        .DependsOn(GenerateReport)
        .Executes(() =>
        {
            // API
            DotNetPublish(s => s
                .SetProject(ProjectApi)
                .SetOutput(TempOutputDirectory / ProjectApi.Name)
                .SetConfiguration(Configuration)
                .SetVersion(FullVersion)
                //.SetSelfContained(true)
                .EnableNoRestore() // Restore is already done in restore task
                .EnableNoBuild()); // Build is already done in compile task

            // MVC
            DotNetPublish(s => s
                .SetProject(ProjectMvc)
                .SetOutput(TempOutputDirectory / ProjectMvc.Name)
                .SetConfiguration(Configuration)
                .SetVersion(FullVersion)
                //.SetSelfContained(true)
                .EnableNoRestore() // Restore is already done in restore task
                .EnableNoBuild()); // Build is already done in compile task

            // DbMigrator
            DotNetPublish(s => s
                .SetProject(ProjectDbMigrator)
                .SetOutput(TempOutputDirectory / ProjectDbMigrator.Name)
                .SetConfiguration(Configuration)
                .SetVersion(FullVersion)
                //.SetSelfContained(true)
                .EnableNoRestore() // Restore is already done in restore task
                .EnableNoBuild()); // Build is already done in compile task
        });

    Target Pack => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            // API
            NuGetPack(s => s
                .SetBasePath(TempOutputDirectory / ProjectApi.Name)
                .SetTargetPath(TempOutputDirectory / ProjectApi.Name / $"{ProjectApi.Name}.nuspec")
                .SetOutputDirectory(ApplicationPackagesDirectory)
                .SetVersion(FullVersion)
                .SetNoPackageAnalysis(true)
                .SetBuild(false)
            );

            // MVC
            NuGetPack(s => s
                .SetBasePath(TempOutputDirectory / ProjectMvc.Name)
                .SetTargetPath(TempOutputDirectory / ProjectMvc.Name / $"{ProjectMvc.Name}.nuspec")
                .SetOutputDirectory(ApplicationPackagesDirectory)
                .SetVersion(FullVersion)
                .SetNoPackageAnalysis(true)
                .SetBuild(false)
            );

            // DbMigrator
            NuGetPack(s => s
                .SetBasePath(TempOutputDirectory / ProjectDbMigrator.Name)
                .SetTargetPath(TempOutputDirectory / ProjectDbMigrator.Name / $"{ProjectDbMigrator.Name}.nuspec")
                .SetOutputDirectory(ApplicationPackagesDirectory)
                .SetVersion(FullVersion)
                .SetNoPackageAnalysis(true)
                .SetBuild(false)
            );
        });

    Target Cleanup => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            TempOutputDirectory.CreateOrCleanDirectory();
        });
}