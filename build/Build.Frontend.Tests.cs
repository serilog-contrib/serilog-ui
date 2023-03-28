using System;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;

partial class Build : NukeBuild
{
    Target Frontend_Tests => _ => _
        .DependsOn(Frontend_Build)
        .Executes(() =>
        {
            NpmTasks.NpmLogger = CustomLogger;

            NpmTasks.NpmRun(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
                .SetCommand("test:local")
            );
        });

    Target Frontend_Tests_Ci => _ => _
    .DependsOn(Frontend_Build)
    .ProceedAfterFailure()
    .Executes(() =>
    {
        NpmTasks.NpmLogger = CustomLogger;

        NpmTasks.NpmRun(s => s
            .SetProcessWorkingDirectory(FrontendWorkingDirectory)
            .SetCommand("test:ci")
        );
    });

    // from https://dev.to/damikun/the-cross-platform-build-automation-with-nuke-1kmc
    public static void CustomLogger(OutputType type, string output)
    {
        switch (type)
        {
            case OutputType.Std:
                Serilog.Log.Debug(output);
                break;
            case OutputType.Err:
            { 
                if (
                    output.Contains(
                        "npmWARN",
                        StringComparison.OrdinalIgnoreCase
                    ) ||
                    output.Contains(
                        "npm WARN",
                        StringComparison.OrdinalIgnoreCase
                    ))
                        Serilog.Log.Warning(output);
                    else
                        Serilog.Log.Error(output);
                    break;
                }
        }
    }
}