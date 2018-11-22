using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public interface ICancelable
    {
        bool Cancel { get; set; }
    }
}
