using System;
using System.Collections.Generic;
using System.Text;

namespace WeaponLib
{
    public interface ICsvSerializable
    {
        bool LoadCSV(string filename);
        bool SaveAsCSV(string filename);
    }
}
