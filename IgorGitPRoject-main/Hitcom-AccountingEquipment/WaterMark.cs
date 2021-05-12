using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hitcom_AccountingEquipment
{
    class WaterMark
    {

    }
    /// <summary>
    /// Блоки создания водяного знака в Textbox (наследует часть опций из Textbox)
    /// </summary>
    public class PlaceTextBox : TextBox
    {

        public string PlaceText
        {
            get => (string)GetValue(PlaceTextProperty);
            set => SetValue(PlaceTextProperty, value);
        }


        public static readonly DependencyProperty PlaceTextProperty =
            DependencyProperty.Register(nameof(PlaceText), typeof(string), typeof(PlaceTextBox),
                new PropertyMetadata("Start typing", (d, e) => ((PlaceTextBox)d).PlaceChanged()));


        /// <summary>Brush for PlaceText</summary>
        public Brush PlaceBrush
        {
            get => (Brush)GetValue(PlaceBrushProperty);
            set => SetValue(PlaceBrushProperty, value);
        }

        public static readonly DependencyProperty PlaceBrushProperty =
            DependencyProperty.Register(nameof(PlaceBrush), typeof(Brush), typeof(PlaceTextBox),
                new PropertyMetadata(Brushes.LightGray, (d, e) => ((PlaceTextBox)d).PlaceChanged()));


        public Thickness PlaceMargin
        {
            get => (Thickness)GetValue(PlaceMarginProperty);
            set => SetValue(PlaceMarginProperty, value);
        }

        public static readonly DependencyProperty PlaceMarginProperty =
            DependencyProperty.Register(nameof(PlaceMargin), typeof(Thickness), typeof(PlaceTextBox),
                new PropertyMetadata(new Thickness(1), (d, e) => ((PlaceTextBox)d).PlaceChanged()));


        public bool IsTextEmpty
        {
            get => (bool)GetValue(IsTextEmptyProperty);
            private set => SetValue(IsTextEmptyPropertyKey, value);
        }

        private static readonly DependencyPropertyKey IsTextEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsTextEmpty), typeof(bool), typeof(PlaceTextBox),
                new PropertyMetadata(true, (d, e) => ((PlaceTextBox)d).IsTextEmptyChanged((bool)e.NewValue)));
        public static readonly DependencyProperty IsTextEmptyProperty = IsTextEmptyPropertyKey.DependencyProperty;

        private void IsTextEmptyChanged(bool newValue)
        {
            IsInChanged = true;
            if (newValue)
            {
                if (Background != BrushPlace)
                    Background = BrushPlace;
            }
            else
            {
                if (Background != BrushClean)
                    Background = BrushClean;
            }
            IsInChanged = false;
        }
        private void PlaceChanged()
        {
            if (double.IsNaN(ActualWidth) || double.IsNaN(ActualHeight) || ActualWidth <= 0 || ActualHeight <= 0)
                return;



            DrawingGroup drawingGroup = new DrawingGroup();

            using (DrawingContext drawingContext = drawingGroup.Open())
            {
                FormattedText formattedText = new FormattedText(
                    PlaceText,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize,
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);


                Geometry textGeometry = formattedText.BuildGeometry(new Point(PlaceMargin.Left, PlaceMargin.Top));


                double width = Math.Max(ActualWidth, formattedText.Width + PlaceMargin.Left + PlaceMargin.Right);
                double height = Math.Max(ActualHeight, formattedText.Height + PlaceMargin.Top + PlaceMargin.Bottom);


                drawingContext.DrawRoundedRectangle(BrushClean, null, new Rect(new Size(width, height)), 0, 0);


                drawingContext.DrawGeometry(PlaceBrush, null, textGeometry);


                BrushPlace = new DrawingBrush(drawingGroup);
            }

            IsTextEmptyChanged(IsTextEmpty);
        }



        public PlaceTextBox() => BrushClean = Background;

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            IsTextEmpty = string.IsNullOrWhiteSpace(Text);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == BackgroundProperty && !IsInChanged)
            {
                BrushClean = Background;
                PlaceChanged();
            }

            else if (
                new DependencyProperty[] { ActualWidthProperty, ActualHeightProperty,
                    FontFamilyProperty, FontStyleProperty, FontWeightProperty,
                    FontStretchProperty, FontSizeProperty }.Contains(e.Property))
                PlaceChanged();
        }


        private Brush BrushClean;
        private Brush BrushPlace;

        private bool IsInChanged;

    }
}
