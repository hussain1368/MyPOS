using Microsoft.Extensions.Localization;
using System.Windows.Markup;
using System;
using POS.WPF.Common;

namespace POS.WPF
{
    public class Labels : MarkupExtension
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        public string Key { get; }

        public Labels(string key)
        {
            Key = key;
            _localizerFactory = ServiceLocator.Current.GetInstance<IStringLocalizerFactory>();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var _t = _localizerFactory.Create(typeof(Labels));
            var value = _t[Key];
            return (string)value;
        }
    }
}
