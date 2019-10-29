using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public class AttributeHost
    {
        private struct Attribute
        {
            public Format Format;
            public int Size;
        }

        AttributeHost()
        {
            _attributes = new List<Attribute>();
        }

        List<Attribute> _attributes;

        AttributeHost PushAttribute(string name, Format format, int size = 0)
        {
            if(size == 0)
            {
                switch(format)
                {
                    case Format.R32G32B32A32_Float:
                        size = sizeof(float) * 4;
                        break;
                    case Format.R32G32B32_Float:
                        size = sizeof(float) * 3;
                        break;
                    case Format.R32G32_Float:
                        size = sizeof(float) * 2;
                        break;
                    case Format.R32_Float:
                        size = sizeof(float);
                        break;
                    case Format.R8G8B8A8_Int:
                    case Format.R8G8B8A8_UInt:
                        size = sizeof(char) * 4;
                        break;
                    case Format.R8G8B8_Int:
                    case Format.R8G8B8_UInt:
                        size = sizeof(char) * 3;
                        break;
                    case Format.R8G8_Int:
                    case Format.R8G8_UInt:
                        size = sizeof(char) * 2;
                        break;
                    case Format.R8_Int:
                    case Format.R8_UInt:
                        size = sizeof(char);
                        break;
                }
            }
            _attributes.Add(new Attribute { Format = format, Size = size });
            return this;
        }
    }
}
