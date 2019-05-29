using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace FrameworkElementExtensions
{
    public class TouchEngagement
    {
        private static readonly bool IsTouchEngagementEnabledDefaultValue = true;

        public static FrameworkElement _engagedElement = null;
        public static List<FrameworkElement> _listeningElements = new List<FrameworkElement>();

        #region IsTouchEngagementEnabled
        public static readonly DependencyProperty IsTouchEngagementEnabledProperty = DependencyProperty.RegisterAttached(
            "IsTouchEngagementEnabled",
            typeof(bool),
            typeof(FrameworkElement),
            new PropertyMetadata(IsTouchEngagementEnabledDefaultValue, new PropertyChangedCallback(OnIsTouchEngagementEnabledChanged)));

        public static void SetIsTouchEngagementEnabled(DependencyObject element, bool value)
        {
            (element as FrameworkElement).SetValue(IsTouchEngagementEnabledProperty, value);
        }

        public static bool GetIsTouchEngagementEnabled(DependencyObject element)
        {
            return (bool)(element as FrameworkElement).GetValue(IsTouchEngagementEnabledProperty);
        }

        private static void OnIsTouchEngagementEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            void handleValueChange(FrameworkElement element, bool? newValue)
            {
                if (!newValue.HasValue || newValue == false)
                {
                    DeregisterPointerListener(element);
                }

                if (newValue.HasValue && newValue == true)
                {
                    RegisterPointerListener(element);
                }
            }

            handleValueChange(sender as FrameworkElement, args.NewValue as bool?);
        }
        #endregion

        private static void RegisterPointerListener(FrameworkElement element)
        {
            if (!_listeningElements.Contains(element))
            {
                element.PointerPressed += Element_PointerPressed;
                element.PointerReleased += Element_PointerReleased;
                element.PointerCaptureLost += Element_PointerCaptureLost;
                element.PointerCanceled += Element_PointerCanceled;
                element.Unloaded += Element_Unloaded;

                _listeningElements.Add(element);
            }
        }

        private static void DeregisterPointerListener(FrameworkElement element)
        {
            if (_listeningElements.Contains(element))
            {
                element.PointerPressed -= Element_PointerPressed;
                element.PointerReleased -= Element_PointerReleased;
                element.PointerCaptureLost -= Element_PointerCaptureLost;
                element.PointerCanceled -= Element_PointerCanceled;
                element.Unloaded -= Element_Unloaded;

                _listeningElements.Remove(element);
            }

            if (_engagedElement == element)
            {
                _engagedElement.ReleasePointerCaptures();
                _engagedElement = null;
            }
        }

        private static void Element_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                DeregisterPointerListener(element);
            }
        }

        private static void Element_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                DeregisterPointerListener(element);
            }
        }

        private static void Element_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                DeregisterPointerListener(element);
            }
        }

        private static void Element_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (_engagedElement == null)
                {
                    // If not engaged, set engaged element
                    element.CapturePointer(e.Pointer);
                    _engagedElement = element;
                }
                else if (_engagedElement == element)
                {
                    // We must be working with the currently engaged element
                    // Nothing to do.
                }
                else
                {
                    // We shouldn't be here...
                    // The _engagedElement should be capturing all pointer events
                    throw new System.Exception("Pointer events are firing when they should be captured instead. Something is wrong.");
                }
            }
        }

        private static void Element_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (_engagedElement == null)
                {
                    // If not engaged, do nothing
                }
                else if (_engagedElement == element)
                {
                    // We must be working with the currently engaged element
                    // Release the pointer capture
                    _engagedElement.ReleasePointerCapture(e.Pointer);
                    _engagedElement = null;
                }
                else
                {
                    // We shouldn't be here...
                    // The _engagedElement should be capturing all pointer events
                    throw new System.Exception("Pointer events are firing when they should be captured instead. Something is wrong.");
                }
            }
        }
    }
}
