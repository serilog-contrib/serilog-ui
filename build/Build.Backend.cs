using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    Target Backend_Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(AbsolutePathExtensions.DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(AbsolutePathExtensions.DeleteDirectory);
            DotNetClean();
        });

    Target Backend_Restore => _ => _
        .DependsOn(Clean, Frontend_Build)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
            );
        });

    Target Backend_Compile => _ => _
        .DependsOn(Backend_Restore, Backend_SonarScan_Start)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });
}