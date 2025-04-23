    namespace BookStore.Common;

    public static class PathUtils
    {
        public static string ProjectRootPath()
        {
            string workingDirectory = Environment.CurrentDirectory;
            return Directory.GetParent(workingDirectory).Parent.FullName;
        }
    }