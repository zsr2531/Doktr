using System;
using System.Xml.Serialization;

namespace Doktr
{
    [Serializable]
    [XmlType(TypeName = "Configuration")]
    public class DoktrConfiguration
    {
        public string Root
        {
            get;
            init;
        } = ".";
        
        public DoktrTarget[] InputFiles
        {
            get;
            init;
        } = Array.Empty<DoktrTarget>();
        
        [XmlArrayItem("Directory")]
        public string[] AdditionalIncludes
        {
            get;
            init;
        } = Array.Empty<string>();

        public string OutputPath
        {
            get;
            init;
        } = "_out";

        public bool UseTablesForMethodParameters
        {
            get;
            init;
        } = true;

        [XmlArrayItem("Url")]
        public string[] XrefUrls
        {
            get;
            init;
        } = Array.Empty<string>();
    }
}