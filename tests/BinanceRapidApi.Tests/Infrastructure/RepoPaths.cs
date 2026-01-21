namespace BinanceRapidApi.Tests.Infrastructure;

internal static class RepoPaths
{
    public static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var gitDir = Path.Combine(dir.FullName, ".git");
            if (Directory.Exists(gitDir))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        // Fallback: current directory (best effort)
        return Directory.GetCurrentDirectory();
    }
}
