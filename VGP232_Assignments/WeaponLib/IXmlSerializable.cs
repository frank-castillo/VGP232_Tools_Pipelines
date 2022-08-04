using System;
using System.Collections.Generic;
using System.Text;

namespace WeaponLib
{
    public interface IXmlSerializable
    {
        bool LoadXML(string filename);
        bool SaveAsXML(string filename);
    }
}
