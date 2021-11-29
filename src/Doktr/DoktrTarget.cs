using System;
using System.Xml.Serialization;

namespace Doktr;

[Serializable]
[XmlType(TypeName = "Target")]
public class DoktrTarget
{
    public string Assembly
    {
        get;
        init;
    } = "";

    public string XmlFile
    {
        get;
        init;
    } = "";
}