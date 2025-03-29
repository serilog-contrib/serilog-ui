using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;


/// <summary><p>Yarn is a package manager that doubles down as project manager. Whether you work on one-shot projects or large monorepos, as a hobbyist or an enterprise user, we've got you covered.</p><p>For more details, visit the <a href="https://yarnpkg.com/">official website</a>.</p></summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[PathTool(Executable = PathExecutable)]
public partial class YarnTasks : ToolTasks, IRequirePathTool
{
    public static string YarnPath { get => new YarnTasks().GetToolPathInternal(); set => new YarnTasks().SetToolPath(value); }
    public const string PathExecutable = "yarn";
    /// <summary><p>Yarn is a package manager that doubles down as project manager. Whether you work on one-shot projects or large monorepos, as a hobbyist or an enterprise user, we've got you covered.</p><p>For more details, visit the <a href="https://yarnpkg.com/">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> Yarn(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new YarnTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Install the project dependencies</p><p>For more details, visit the <a href="https://yarnpkg.com/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--check-cache</c> via <see cref="YarnInstallSettings.CheckCache"/></li><li><c>--immutable</c> via <see cref="YarnInstallSettings.Immutable"/></li><li><c>--immutable-cache</c> via <see cref="YarnInstallSettings.ImmutableCache"/></li><li><c>--inline-builds</c> via <see cref="YarnInstallSettings.InlineBuilds"/></li><li><c>--json</c> via <see cref="YarnInstallSettings.Json"/></li><li><c>--mode</c> via <see cref="YarnInstallSettings.Mode"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> YarnInstall(YarnInstallSettings options = null) => new YarnTasks().Run<YarnInstallSettings>(options);
    /// <inheritdoc cref="YarnTasks.YarnInstall(.YarnInstallSettings)"/>
    public static IReadOnlyCollection<Output> YarnInstall(Configure<YarnInstallSettings> configurator) => new YarnTasks().Run<YarnInstallSettings>(configurator.Invoke(new YarnInstallSettings()));
    /// <inheritdoc cref="YarnTasks.YarnInstall(.YarnInstallSettings)"/>
    public static IEnumerable<(YarnInstallSettings Settings, IReadOnlyCollection<Output> Output)> YarnInstall(CombinatorialConfigure<YarnInstallSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(YarnInstall, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Run a script defined in the package.json</p><p>For more details, visit the <a href="https://yarnpkg.com/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;arguments&gt;</c> via <see cref="YarnRunSettings.Arguments"/></li><li><c>&lt;command&gt;</c> via <see cref="YarnRunSettings.Command"/></li><li><c>--binaries-only</c> via <see cref="YarnRunSettings.BinariesOnly"/></li><li><c>--top-level</c> via <see cref="YarnRunSettings.TopLevel"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> YarnRun(YarnRunSettings options = null) => new YarnTasks().Run<YarnRunSettings>(options);
    /// <inheritdoc cref="YarnTasks.YarnRun(.YarnRunSettings)"/>
    public static IReadOnlyCollection<Output> YarnRun(Configure<YarnRunSettings> configurator) => new YarnTasks().Run<YarnRunSettings>(configurator.Invoke(new YarnRunSettings()));
    /// <inheritdoc cref="YarnTasks.YarnRun(.YarnRunSettings)"/>
    public static IEnumerable<(YarnRunSettings Settings, IReadOnlyCollection<Output> Output)> YarnRun(CombinatorialConfigure<YarnRunSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(YarnRun, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Read a configuration settings</p><p>For more details, visit the <a href="https://yarnpkg.com/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;name&gt;</c> via <see cref="YarnGetConfigSettings.Name"/></li><li><c>--json</c> via <see cref="YarnGetConfigSettings.Json"/></li><li><c>--no-redacted</c> via <see cref="YarnGetConfigSettings.NoRedacted"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> YarnGetConfig(YarnGetConfigSettings options = null) => new YarnTasks().Run<YarnGetConfigSettings>(options);
    /// <inheritdoc cref="YarnTasks.YarnGetConfig(.YarnGetConfigSettings)"/>
    public static IReadOnlyCollection<Output> YarnGetConfig(Configure<YarnGetConfigSettings> configurator) => new YarnTasks().Run<YarnGetConfigSettings>(configurator.Invoke(new YarnGetConfigSettings()));
    /// <inheritdoc cref="YarnTasks.YarnGetConfig(.YarnGetConfigSettings)"/>
    public static IEnumerable<(YarnGetConfigSettings Settings, IReadOnlyCollection<Output> Output)> YarnGetConfig(CombinatorialConfigure<YarnGetConfigSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(YarnGetConfig, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Change a configuration settings</p><p>For more details, visit the <a href="https://yarnpkg.com/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;name&gt;</c> via <see cref="YarnSetConfigSettings.Name"/></li><li><c>&lt;value&gt;</c> via <see cref="YarnSetConfigSettings.Value"/></li><li><c>--home</c> via <see cref="YarnSetConfigSettings.Home"/></li><li><c>--json</c> via <see cref="YarnSetConfigSettings.Json"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> YarnSetConfig(YarnSetConfigSettings options = null) => new YarnTasks().Run<YarnSetConfigSettings>(options);
    /// <inheritdoc cref="YarnTasks.YarnSetConfig(.YarnSetConfigSettings)"/>
    public static IReadOnlyCollection<Output> YarnSetConfig(Configure<YarnSetConfigSettings> configurator) => new YarnTasks().Run<YarnSetConfigSettings>(configurator.Invoke(new YarnSetConfigSettings()));
    /// <inheritdoc cref="YarnTasks.YarnSetConfig(.YarnSetConfigSettings)"/>
    public static IEnumerable<(YarnSetConfigSettings Settings, IReadOnlyCollection<Output> Output)> YarnSetConfig(CombinatorialConfigure<YarnSetConfigSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(YarnSetConfig, degreeOfParallelism, completeOnFailure);
}
#region YarnInstallSettings
/// <inheritdoc cref="YarnTasks.YarnInstall(.YarnInstallSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(YarnTasks), Command = nameof(YarnTasks.YarnInstall), Arguments = "install")]
public partial class YarnInstallSettings : ToolOptions
{
    /// <summary>Format the output as an NDJSON stream.</summary>
    [Argument(Format = "--json")] public bool? Json => Get<bool?>(() => Json);
    /// <summary>Abort with an error exit code if the lockfile was to be modified.</summary>
    [Argument(Format = "--immutable")] public bool? Immutable => Get<bool?>(() => Immutable);
    /// <summary>Abort with an error exit code if the cache folder was to be modified.</summary>
    [Argument(Format = "--immutable-cache")] public bool? ImmutableCache => Get<bool?>(() => ImmutableCache);
    /// <summary>Always refetch the packages and ensure that their checksums are consistent.</summary>
    [Argument(Format = "--check-cache")] public bool? CheckCache => Get<bool?>(() => CheckCache);
    /// <summary>Verbosely print the output of the build steps of dependencies.</summary>
    [Argument(Format = "--inline-builds")] public bool? InlineBuilds => Get<bool?>(() => InlineBuilds);
    /// <summary>If the <c>--mode=<mode></c> option is set, Yarn will change which artifacts are generated.</summary>
    [Argument(Format = "--mode={value}")] public YarnInstallMode Mode => Get<YarnInstallMode>(() => Mode);
}
#endregion
#region YarnRunSettings
/// <inheritdoc cref="YarnTasks.YarnRun(.YarnRunSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(YarnTasks), Command = nameof(YarnTasks.YarnRun), Arguments = "run")]
public partial class YarnRunSettings : ToolOptions
{
    /// <summary>The command to be executed.</summary>
    [Argument(Format = "{value}")] public string Command => Get<string>(() => Command);
    /// <summary>Arguments passed to the script.</summary>
    [Argument(Format = "{value}", Separator = " ")] public IReadOnlyList<string> Arguments => Get<List<string>>(() => Arguments);
    /// <summary>Check the root workspace for scripts and/or binaries instead of the current one.</summary>
    [Argument(Format = "--top-level")] public bool? TopLevel => Get<bool?>(() => TopLevel);
    /// <summary>Ignore any user defined scripts and only check for binaries.</summary>
    [Argument(Format = "--binaries-only")] public bool? BinariesOnly => Get<bool?>(() => BinariesOnly);
}
#endregion
#region YarnGetConfigSettings
/// <inheritdoc cref="YarnTasks.YarnGetConfig(.YarnGetConfigSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(YarnTasks), Command = nameof(YarnTasks.YarnGetConfig), Arguments = "config get")]
public partial class YarnGetConfigSettings : ToolOptions
{
    /// <summary>The name of the configuration setting.</summary>
    [Argument(Format = "{value}")] public string Name => Get<string>(() => Name);
    /// <summary>Format the output as an NDJSON stream.</summary>
    [Argument(Format = "--json")] public bool? Json => Get<bool?>(() => Json);
    /// <summary>Don't redact secrets (such as tokens) from the output.</summary>
    [Argument(Format = "--no-redacted")] public bool? NoRedacted => Get<bool?>(() => NoRedacted);
}
#endregion
#region YarnSetConfigSettings
/// <inheritdoc cref="YarnTasks.YarnSetConfig(.YarnSetConfigSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(YarnTasks), Command = nameof(YarnTasks.YarnSetConfig), Arguments = "config set")]
public partial class YarnSetConfigSettings : ToolOptions
{
    /// <summary>The name of the configuration setting.</summary>
    [Argument(Format = "{value}")] public string Name => Get<string>(() => Name);
    /// <summary>Set complex configuration settings to JSON values.</summary>
    [Argument(Format = "--json")] public bool? Json => Get<bool?>(() => Json);
    /// <summary>The value of the configuration setting.</summary>
    [Argument(Format = "{value}")] public string Value => Get<string>(() => Value);
    /// <summary>Update the home configuration instead of the project configuration.</summary>
    [Argument(Format = "--home")] public bool? Home => Get<bool?>(() => Home);
}
#endregion
#region YarnInstallSettingsExtensions
/// <inheritdoc cref="YarnTasks.YarnInstall(.YarnInstallSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class YarnInstallSettingsExtensions
{
    #region Json
    /// <inheritdoc cref="YarnInstallSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Json))]
    public static T SetJson<T>(this T o, bool? v) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Json, v));
    /// <inheritdoc cref="YarnInstallSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Json))]
    public static T ResetJson<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Remove(() => o.Json));
    /// <inheritdoc cref="YarnInstallSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Json))]
    public static T EnableJson<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Json, true));
    /// <inheritdoc cref="YarnInstallSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Json))]
    public static T DisableJson<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Json, false));
    /// <inheritdoc cref="YarnInstallSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Json))]
    public static T ToggleJson<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Json, !o.Json));
    #endregion
    #region Immutable
    /// <inheritdoc cref="YarnInstallSettings.Immutable"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Immutable))]
    public static T SetImmutable<T>(this T o, bool? v) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Immutable, v));
    /// <inheritdoc cref="YarnInstallSettings.Immutable"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Immutable))]
    public static T ResetImmutable<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Remove(() => o.Immutable));
    /// <inheritdoc cref="YarnInstallSettings.Immutable"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Immutable))]
    public static T EnableImmutable<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Immutable, true));
    /// <inheritdoc cref="YarnInstallSettings.Immutable"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Immutable))]
    public static T DisableImmutable<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Immutable, false));
    /// <inheritdoc cref="YarnInstallSettings.Immutable"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Immutable))]
    public static T ToggleImmutable<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Immutable, !o.Immutable));
    #endregion
    #region ImmutableCache
    /// <inheritdoc cref="YarnInstallSettings.ImmutableCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.ImmutableCache))]
    public static T SetImmutableCache<T>(this T o, bool? v) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.ImmutableCache, v));
    /// <inheritdoc cref="YarnInstallSettings.ImmutableCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.ImmutableCache))]
    public static T ResetImmutableCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Remove(() => o.ImmutableCache));
    /// <inheritdoc cref="YarnInstallSettings.ImmutableCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.ImmutableCache))]
    public static T EnableImmutableCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.ImmutableCache, true));
    /// <inheritdoc cref="YarnInstallSettings.ImmutableCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.ImmutableCache))]
    public static T DisableImmutableCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.ImmutableCache, false));
    /// <inheritdoc cref="YarnInstallSettings.ImmutableCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.ImmutableCache))]
    public static T ToggleImmutableCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.ImmutableCache, !o.ImmutableCache));
    #endregion
    #region CheckCache
    /// <inheritdoc cref="YarnInstallSettings.CheckCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.CheckCache))]
    public static T SetCheckCache<T>(this T o, bool? v) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.CheckCache, v));
    /// <inheritdoc cref="YarnInstallSettings.CheckCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.CheckCache))]
    public static T ResetCheckCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Remove(() => o.CheckCache));
    /// <inheritdoc cref="YarnInstallSettings.CheckCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.CheckCache))]
    public static T EnableCheckCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.CheckCache, true));
    /// <inheritdoc cref="YarnInstallSettings.CheckCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.CheckCache))]
    public static T DisableCheckCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.CheckCache, false));
    /// <inheritdoc cref="YarnInstallSettings.CheckCache"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.CheckCache))]
    public static T ToggleCheckCache<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.CheckCache, !o.CheckCache));
    #endregion
    #region InlineBuilds
    /// <inheritdoc cref="YarnInstallSettings.InlineBuilds"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.InlineBuilds))]
    public static T SetInlineBuilds<T>(this T o, bool? v) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.InlineBuilds, v));
    /// <inheritdoc cref="YarnInstallSettings.InlineBuilds"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.InlineBuilds))]
    public static T ResetInlineBuilds<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Remove(() => o.InlineBuilds));
    /// <inheritdoc cref="YarnInstallSettings.InlineBuilds"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.InlineBuilds))]
    public static T EnableInlineBuilds<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.InlineBuilds, true));
    /// <inheritdoc cref="YarnInstallSettings.InlineBuilds"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.InlineBuilds))]
    public static T DisableInlineBuilds<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.InlineBuilds, false));
    /// <inheritdoc cref="YarnInstallSettings.InlineBuilds"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.InlineBuilds))]
    public static T ToggleInlineBuilds<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.InlineBuilds, !o.InlineBuilds));
    #endregion
    #region Mode
    /// <inheritdoc cref="YarnInstallSettings.Mode"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Mode))]
    public static T SetMode<T>(this T o, YarnInstallMode v) where T : YarnInstallSettings => o.Modify(b => b.Set(() => o.Mode, v));
    /// <inheritdoc cref="YarnInstallSettings.Mode"/>
    [Pure]
    [Builder(Type = typeof(YarnInstallSettings), Property = nameof(YarnInstallSettings.Mode))]
    public static T ResetMode<T>(this T o) where T : YarnInstallSettings => o.Modify(b => b.Remove(() => o.Mode));
    #endregion
}
#endregion
#region YarnRunSettingsExtensions
/// <inheritdoc cref="YarnTasks.YarnRun(.YarnRunSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class YarnRunSettingsExtensions
{
    #region Command
    /// <inheritdoc cref="YarnRunSettings.Command"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Command))]
    public static T SetCommand<T>(this T o, string v) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.Command, v));
    /// <inheritdoc cref="YarnRunSettings.Command"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Command))]
    public static T ResetCommand<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Remove(() => o.Command));
    #endregion
    #region Arguments
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T SetArguments<T>(this T o, params string[] v) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T SetArguments<T>(this T o, IEnumerable<string> v) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T AddArguments<T>(this T o, params string[] v) where T : YarnRunSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T AddArguments<T>(this T o, IEnumerable<string> v) where T : YarnRunSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, params string[] v) where T : YarnRunSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, IEnumerable<string> v) where T : YarnRunSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="YarnRunSettings.Arguments"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.Arguments))]
    public static T ClearArguments<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.ClearCollection(() => o.Arguments));
    #endregion
    #region TopLevel
    /// <inheritdoc cref="YarnRunSettings.TopLevel"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.TopLevel))]
    public static T SetTopLevel<T>(this T o, bool? v) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.TopLevel, v));
    /// <inheritdoc cref="YarnRunSettings.TopLevel"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.TopLevel))]
    public static T ResetTopLevel<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Remove(() => o.TopLevel));
    /// <inheritdoc cref="YarnRunSettings.TopLevel"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.TopLevel))]
    public static T EnableTopLevel<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.TopLevel, true));
    /// <inheritdoc cref="YarnRunSettings.TopLevel"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.TopLevel))]
    public static T DisableTopLevel<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.TopLevel, false));
    /// <inheritdoc cref="YarnRunSettings.TopLevel"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.TopLevel))]
    public static T ToggleTopLevel<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.TopLevel, !o.TopLevel));
    #endregion
    #region BinariesOnly
    /// <inheritdoc cref="YarnRunSettings.BinariesOnly"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.BinariesOnly))]
    public static T SetBinariesOnly<T>(this T o, bool? v) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.BinariesOnly, v));
    /// <inheritdoc cref="YarnRunSettings.BinariesOnly"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.BinariesOnly))]
    public static T ResetBinariesOnly<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Remove(() => o.BinariesOnly));
    /// <inheritdoc cref="YarnRunSettings.BinariesOnly"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.BinariesOnly))]
    public static T EnableBinariesOnly<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.BinariesOnly, true));
    /// <inheritdoc cref="YarnRunSettings.BinariesOnly"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.BinariesOnly))]
    public static T DisableBinariesOnly<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.BinariesOnly, false));
    /// <inheritdoc cref="YarnRunSettings.BinariesOnly"/>
    [Pure]
    [Builder(Type = typeof(YarnRunSettings), Property = nameof(YarnRunSettings.BinariesOnly))]
    public static T ToggleBinariesOnly<T>(this T o) where T : YarnRunSettings => o.Modify(b => b.Set(() => o.BinariesOnly, !o.BinariesOnly));
    #endregion
}
#endregion
#region YarnGetConfigSettingsExtensions
/// <inheritdoc cref="YarnTasks.YarnGetConfig(.YarnGetConfigSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class YarnGetConfigSettingsExtensions
{
    #region Name
    /// <inheritdoc cref="YarnGetConfigSettings.Name"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Name))]
    public static T SetName<T>(this T o, string v) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.Name, v));
    /// <inheritdoc cref="YarnGetConfigSettings.Name"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Name))]
    public static T ResetName<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Remove(() => o.Name));
    #endregion
    #region Json
    /// <inheritdoc cref="YarnGetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Json))]
    public static T SetJson<T>(this T o, bool? v) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.Json, v));
    /// <inheritdoc cref="YarnGetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Json))]
    public static T ResetJson<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Remove(() => o.Json));
    /// <inheritdoc cref="YarnGetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Json))]
    public static T EnableJson<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.Json, true));
    /// <inheritdoc cref="YarnGetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Json))]
    public static T DisableJson<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.Json, false));
    /// <inheritdoc cref="YarnGetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.Json))]
    public static T ToggleJson<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.Json, !o.Json));
    #endregion
    #region NoRedacted
    /// <inheritdoc cref="YarnGetConfigSettings.NoRedacted"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.NoRedacted))]
    public static T SetNoRedacted<T>(this T o, bool? v) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.NoRedacted, v));
    /// <inheritdoc cref="YarnGetConfigSettings.NoRedacted"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.NoRedacted))]
    public static T ResetNoRedacted<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Remove(() => o.NoRedacted));
    /// <inheritdoc cref="YarnGetConfigSettings.NoRedacted"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.NoRedacted))]
    public static T EnableNoRedacted<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.NoRedacted, true));
    /// <inheritdoc cref="YarnGetConfigSettings.NoRedacted"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.NoRedacted))]
    public static T DisableNoRedacted<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.NoRedacted, false));
    /// <inheritdoc cref="YarnGetConfigSettings.NoRedacted"/>
    [Pure]
    [Builder(Type = typeof(YarnGetConfigSettings), Property = nameof(YarnGetConfigSettings.NoRedacted))]
    public static T ToggleNoRedacted<T>(this T o) where T : YarnGetConfigSettings => o.Modify(b => b.Set(() => o.NoRedacted, !o.NoRedacted));
    #endregion
}
#endregion
#region YarnSetConfigSettingsExtensions
/// <inheritdoc cref="YarnTasks.YarnSetConfig(.YarnSetConfigSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class YarnSetConfigSettingsExtensions
{
    #region Name
    /// <inheritdoc cref="YarnSetConfigSettings.Name"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Name))]
    public static T SetName<T>(this T o, string v) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Name, v));
    /// <inheritdoc cref="YarnSetConfigSettings.Name"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Name))]
    public static T ResetName<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Remove(() => o.Name));
    #endregion
    #region Json
    /// <inheritdoc cref="YarnSetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Json))]
    public static T SetJson<T>(this T o, bool? v) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Json, v));
    /// <inheritdoc cref="YarnSetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Json))]
    public static T ResetJson<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Remove(() => o.Json));
    /// <inheritdoc cref="YarnSetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Json))]
    public static T EnableJson<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Json, true));
    /// <inheritdoc cref="YarnSetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Json))]
    public static T DisableJson<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Json, false));
    /// <inheritdoc cref="YarnSetConfigSettings.Json"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Json))]
    public static T ToggleJson<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Json, !o.Json));
    #endregion
    #region Value
    /// <inheritdoc cref="YarnSetConfigSettings.Value"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Value))]
    public static T SetValue<T>(this T o, string v) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Value, v));
    /// <inheritdoc cref="YarnSetConfigSettings.Value"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Value))]
    public static T ResetValue<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Remove(() => o.Value));
    #endregion
    #region Home
    /// <inheritdoc cref="YarnSetConfigSettings.Home"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Home))]
    public static T SetHome<T>(this T o, bool? v) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Home, v));
    /// <inheritdoc cref="YarnSetConfigSettings.Home"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Home))]
    public static T ResetHome<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Remove(() => o.Home));
    /// <inheritdoc cref="YarnSetConfigSettings.Home"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Home))]
    public static T EnableHome<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Home, true));
    /// <inheritdoc cref="YarnSetConfigSettings.Home"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Home))]
    public static T DisableHome<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Home, false));
    /// <inheritdoc cref="YarnSetConfigSettings.Home"/>
    [Pure]
    [Builder(Type = typeof(YarnSetConfigSettings), Property = nameof(YarnSetConfigSettings.Home))]
    public static T ToggleHome<T>(this T o) where T : YarnSetConfigSettings => o.Modify(b => b.Set(() => o.Home, !o.Home));
    #endregion
}
#endregion
#region YarnInstallMode
/// <summary>Used within <see cref="YarnTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<YarnInstallMode>))]
public partial class YarnInstallMode : Enumeration
{
    public static YarnInstallMode skip_build = (YarnInstallMode)"skip-build";
    public static YarnInstallMode update_lockfile = (YarnInstallMode)"update-lockfile";
    public static implicit operator YarnInstallMode(string value)
    {
        return new YarnInstallMode { Value = value };
    }
}
#endregion
