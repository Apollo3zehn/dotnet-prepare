using System.Reflection;

Console.WriteLine("Welcome to Apollo3zehn's project initializer.");
Walk();

static void Walk(string root = ".")
{
    if (IsGitRepository(root))
    {
        Console.WriteLine($"Directory {root} is a GIT repository. Prepare repository now.");

        var originalDir = Environment.CurrentDirectory;

        try
        {
            Environment.CurrentDirectory = root;
            Prepare();
        }
        finally
        {
            Environment.CurrentDirectory = originalDir;
        }
    }

    else
    {
        Console.WriteLine($"Directory {root} is not a GIT repository. Iterate through sub repositories now.");
        var any = false;

        foreach (var directory in Directory.EnumerateDirectories("."))
        {
            any = true;
            Walk(directory);
        }

        if (!any && root == "")
            Console.WriteLine($"No GIT repositories found. Bye Bye.");
    }
}

static bool IsGitRepository(string root)
{
    return Directory.Exists(Path.Combine(root, ".git"));
}

static void Prepare()
{
    const string SOLUTION = "solution.json";
    const string VERSION = "version.json";
    const string CHANGELOG = "CHANGELOG.md";

    var resourceFolderPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "..", "resources");

    if (!File.Exists(SOLUTION))
    {
        Console.WriteLine($"Create file {SOLUTION}");
        File.Copy(Path.Combine(resourceFolderPath, SOLUTION), SOLUTION);
    }

    if (!File.Exists(VERSION))
    {
        Console.WriteLine($"Create file {SOLUTION}");
        File.Copy(Path.Combine(resourceFolderPath, VERSION), VERSION);
    }

    if (!File.Exists(CHANGELOG))
    {
        Console.WriteLine($"Create file {CHANGELOG}");
        File.Copy(Path.Combine(resourceFolderPath, CHANGELOG), CHANGELOG);
    }

    foreach (var filePath in Directory.EnumerateFiles(resourceFolderPath, "*.*", SearchOption.AllDirectories))
    {
        if (filePath.EndsWith(SOLUTION) || filePath.EndsWith(VERSION) || filePath.EndsWith(CHANGELOG))
            continue;

        var targetFilePath = filePath[(resourceFolderPath.Length + 1)..];
        var targetFolderPath = Path.GetDirectoryName(targetFilePath);

        if (!string.IsNullOrWhiteSpace(targetFolderPath))
            Directory.CreateDirectory(targetFolderPath);

        Console.WriteLine($"Create file {filePath[(resourceFolderPath.Length + 1)..]}");
        File.Copy(filePath, targetFilePath, overwrite: true);
    }

    Console.WriteLine($"Done.");
}
