using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2a
{
    public interface ICsvSerializable
    {
        bool LoadCSV(string filename);
        bool SaveAsCSV(string filename);
    }
}
