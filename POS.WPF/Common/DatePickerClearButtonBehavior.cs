using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;

namespace POS.WPF.Common
{
    public class DatePickerClearButtonBehavior : Behavior<DatePicker>
    {
        private Button _clearButton;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            // Also try to react if template is reapplied
            AssociatedObject.TargetUpdated += AssociatedObject_TargetUpdated;
        }

        protected override void OnDetaching()
        {
            DetachClearButton();
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            AssociatedObject.TargetUpdated -= AssociatedObject_TargetUpdated;
            base.OnDetaching();
        }

        private void AssociatedObject_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            // Template might have changed; re-wire
            TryAttachClearButton();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            TryAttachClearButton();
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            DetachClearButton();
        }

        private void TryAttachClearButton()
        {
            if (AssociatedObject == null)
                return;

            // Find the clear button by conventional template name used by MDIX
            var found = FindDescendantByName<Button>(AssociatedObject, "PART_ClearButton");
            if (found == null)
                return;

            // If we already attached to this button, nothing to do
            if (_clearButton == found)
                return;

            // Detach old
            DetachClearButton();

            _clearButton = found;

            // Defensive template tweaks
            try
            {
                _clearButton.ClickMode = System.Windows.Controls.ClickMode.Release;
                _clearButton.Focusable = false;
                _clearButton.IsTabStop = false;
            }
            catch
            {
                // Swallow any template read-only exceptions
            }

            _clearButton.Click -= ClearButton_Click;
            _clearButton.Click += ClearButton_Click;
        }

        private void DetachClearButton()
        {
            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButton_Click;
                _clearButton = null;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Only perform the SelectedDate mutation after WPF completes input/focus/popup work
            if (AssociatedObject == null)
                return;

            AssociatedObject.Dispatcher.BeginInvoke(new Action(() =>
            {
                // Double-check AssociatedObject still exists
                if (AssociatedObject == null)
                    return;

                try
                {
                    AssociatedObject.SelectedDate = null;
                }
                catch
                {
                    // ignore any unexpected template/state exceptions
                }

            }), DispatcherPriority.ApplicationIdle);
        }

        // Utility: find first descendant of given type with a specific name
        private static T FindDescendantByName<T>(DependencyObject root, string name) where T : FrameworkElement
        {
            if (root == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child is T fe && fe.Name == name)
                    return fe;

                var result = FindDescendantByName<T>(child, name);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
