
using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace Dnvm;

public static class ListCommand
{
    /// <summary>
    /// Prints a list of installed SDK versions and their locations.
    public static Task<int> Run(Logger logger, DnvmFs home)
    {
        Manifest manifest;
        try
        {
            manifest = home.ReadManifest();
        }
        catch (Exception e)
        {
            logger.Error("Error reading manifest: " + e.Message);
            return Task.FromResult(1);
        }

        PrintSdks(logger, manifest);

        return Task.FromResult(0);
    }

    public static void PrintSdks(Logger logger, Manifest manifest)
    {
        logger.Log("Installed SDKs:");
        logger.Log();
        var table = new Table();
        table.AddColumn(new TableColumn(" "));
        table.AddColumn("Channel");
        table.AddColumn("Version");
        table.AddColumn("Location");
        foreach (var channel in manifest.TrackedChannels)
        {
            string selected = manifest.CurrentSdkDir == channel.SdkDirName ? "*" : " ";
            foreach (var version in channel.InstalledSdkVersions)
            {
                table.AddRow(selected, channel.ChannelName.ToString(), version, channel.SdkDirName.Name);
            }
        }
        logger.Console.Write(table);
    }
}