namespace Doktr.CommandLine
{
    public class TargetFiles
    {
        public TargetFiles(string assembly, string xmldoc)
        {
            Assembly = assembly;
            Xmldoc = xmldoc;
        }

        public string Assembly
        {
            get;
        }

        public string Xmldoc
        {
            get;
        }
    }
}