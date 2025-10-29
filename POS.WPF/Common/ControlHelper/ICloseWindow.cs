using System;

namespace POS.WPF.Common.ControlHelper
{
    interface ICloseWindow
    {
        public Action Close { get; set; }
    }
}
