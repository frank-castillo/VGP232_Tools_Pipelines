using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2a
{
    public interface IJsonSerializable
    {
        bool LoadJSON(string filename);
        bool SaveAsJSON(string filename);
    }
}
