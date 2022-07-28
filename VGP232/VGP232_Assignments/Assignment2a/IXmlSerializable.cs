using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2a
{
    public interface IXmlSerializable
    {
        bool LoadXML(string filename);
        bool SaveAsXML(string filename);
    }
}
