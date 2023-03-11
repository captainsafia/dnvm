
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Serde.Json;

namespace Dnvm;

public static class SelectCommand
{
    public static async Task<int> Run(DnvmFs dnvmFs, GlobalOptions options, Logger logger, CommandArguments.SelectArguments args)
    {
        var newDir = new SdkDirName(args.SdkDirName);
        var manifest = ManifestUtils.ReadOrCreateManifest(dnvmFs);
        manifest = await SelectNewDir(options.DnvmHome, newDir, manifest);
        dnvmFs.WriteManifest(manifest);
        return 0;
    }

    /// <summary>
    /// Replaces the dotnet symlink with one pointing to the new SDK and
    /// updates the manifest to reflect the new SDK dir.
    /// </summary>
    public static Task<Manifest> SelectNewDir(string dnvmHome, SdkDirName newDir, Manifest manifest)
    {
        InstallCommand.RetargetSymlink(dnvmHome, newDir);
        return Task.FromResult(manifest with { CurrentSdkDir = newDir });
    }
}