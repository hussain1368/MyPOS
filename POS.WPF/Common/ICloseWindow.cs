using System;

namespace POS.WPF.Common
{
    interface ICloseWindow
    {
        public Action Close { get; set; }
    }
}
