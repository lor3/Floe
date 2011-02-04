﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Floe.UI
{
	[ValueConversion(typeof(SolidColorBrush), typeof(Color))]
	public class BrushToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var scb = value as SolidColorBrush;
			if (scb != null && targetType == typeof(Color))
			{
				var c = scb.Color;
				c.A = (byte)((double)scb.Color.A * scb.Opacity);
				return c;
			}
			else
			{
				throw new ArgumentException();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class DoubleToPercentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((double)value).ToString("P0");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return double.Parse(value.ToString());
		}
	}

	public class BytesToFriendlyStringConverter : IValueConverter
	{
		private static readonly string[] _suffixes = new[] { "B", "KB", "MB", "GB", "TB" };
		private static readonly string[] _formats = new[] { "F0", "F0", "F1", "F2", "F2" };

		private string _format = "{0} {1}";

		public string Format { get { return _format; } set { _format = value; } }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var bytes = (double)value;
			int i = 0;
			while (bytes > 1024 && i < _suffixes.Length - 1)
			{
				bytes /= 1024.0;
				i++;
			}
			return string.Format(_format, bytes.ToString(_formats[i]), _suffixes[i]);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class BrushAlphaConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var brush = (SolidColorBrush)values[0];
			double alpha = (double)values[1];
			return new SolidColorBrush(Color.FromArgb((byte)(alpha * 255.0), brush.Color.R, brush.Color.G, brush.Color.B));
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(SolidColorBrush), typeof(SolidColorBrush))]
	public class BrushSaturationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var hsv = HsvColor.FromColor(((SolidColorBrush)value).Color);
			hsv = new HsvColor(hsv.A, hsv.H, hsv.S * (float)(double)parameter, hsv.V);
			return new SolidColorBrush(hsv.ToColor());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
