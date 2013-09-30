namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public sealed class ScopeProgress
    {
        public string Name { get; private set; }

        public double Progress { get; private set; }
        
        public ScopeProgress(string name, double progress)
        {
            Name = name;
            Progress = progress;
        }
    }
}