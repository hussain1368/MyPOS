using System;

namespace POS.WPF
{
    interface ICloseWindow
    {
        public Action Close { get; set; }
    }
}
