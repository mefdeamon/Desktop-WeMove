#region Deamon
// the app's classes of wemove project
#endregion

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deamon.UiCore.Controls
{
    /// <summary>
    /// The button with a icon
    /// </summary>
    public class IconButton : Button
    {
        public IconButton()
        {
            DefaultStyleKey = typeof(IconButton);
        }

        /// <summary>
        /// The icon geometry data
        /// </summary>
        public Geometry IconData
        {
            get => (Geometry)GetValue(IconDataProperty);
            set => SetValue(IconDataProperty, value);
        }

        /// <summary>
        /// <see cref="IconData"/>
        /// </summary>
        public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register(nameof(IconData), typeof(Geometry), typeof(IconButton));


        /// <summary>
        /// Rotate Transform Angle
        /// </summary>
        public double RotateAngle
        {
            get { return (double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }

        /// <summary>
        /// <see cref="RotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register(nameof(RotateAngle), typeof(double), typeof(IconButton), new PropertyMetadata(default(double)));



    }
}
