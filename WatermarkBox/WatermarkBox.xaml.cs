using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;

namespace WatermarkBox
{
    /// <summary>
    /// Interaction logic for WatermarkBox.xaml
    /// </summary>
    public partial class WatermarkBox : UserControl
    {
		#region --- Custom Properties ---
		public static readonly DependencyProperty WatermarkBackColorProperty =
			DependencyProperty.Register("WatermarkBackColor", typeof(SolidColorBrush),
				typeof(WatermarkBox), new PropertyMetadata(new SolidColorBrush(Colors.White)));
		public SolidColorBrush WatermarkBackColor
		{
			get { return (SolidColorBrush)GetValue(WatermarkBackColorProperty); }
			set { SetValue(WatermarkBackColorProperty, value); }
		}

		public static readonly DependencyProperty WatermarkTextColorProperty =
			DependencyProperty.Register("WatermarkTextColor", typeof(SolidColorBrush),
				typeof(WatermarkBox), new PropertyMetadata(new SolidColorBrush(Colors.LightSteelBlue)));
		public SolidColorBrush WatermarkTextColor
		{
			get { return (SolidColorBrush)GetValue(WatermarkTextColorProperty); }
			set { SetValue(WatermarkTextColorProperty, value); }
		}

		public static readonly DependencyProperty WatermarkBorderColorProperty =
			DependencyProperty.Register("WatermarkBorderColor", typeof(SolidColorBrush),
				typeof(WatermarkBox), new PropertyMetadata(new SolidColorBrush(Colors.Indigo)));
		public SolidColorBrush WatermarkBorderColor
		{
			get { return (SolidColorBrush)GetValue(WatermarkBorderColorProperty); }
			set { SetValue(WatermarkBorderColorProperty, value); }
		}

		public static readonly DependencyProperty BoxBordersProperty =
			DependencyProperty.Register("BoxBorders", typeof(Thickness),
				typeof(WatermarkBox), new PropertyMetadata(new Thickness(1, 1, 1, 1)));
		public Thickness BoxBorders
		{
			get { return (Thickness)GetValue(BoxBordersProperty); }
			set { SetValue(BoxBordersProperty, value); }
		}

		public static readonly DependencyProperty TextColorProperty =
			DependencyProperty.Register("TextColor", typeof(SolidColorBrush),
				typeof(WatermarkBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
		public SolidColorBrush TextColor
		{
			get { return (SolidColorBrush)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		public static readonly DependencyProperty PromptProperty =
			DependencyProperty.Register("Prompt", typeof(string),
				typeof(WatermarkBox), new PropertyMetadata("Watermark Box"));
		public string Prompt
		{
			get { return (string)GetValue(PromptProperty); }
			set { SetValue(PromptProperty, value); }
		}

		public static readonly DependencyProperty HorizontalAlignProperty =
			DependencyProperty.Register("HorizontalAlign", typeof(HorizontalAlignment),
				typeof(WatermarkBox), new PropertyMetadata(HorizontalAlignment.Stretch));
		public HorizontalAlignment HorizontalAlign
		{
			get { return (HorizontalAlignment)GetValue(HorizontalAlignProperty); }
			set { SetValue(HorizontalAlignProperty, value); }
		}

		public static readonly DependencyProperty HorizontalPromptAlignProperty =
			DependencyProperty.Register("HorizontalPromptAlign", typeof(TextAlignment),
				typeof(WatermarkBox), new PropertyMetadata(TextAlignment.Justify));
		public TextAlignment HorizontalPromptAlign
		{
			get { return (TextAlignment)GetValue(HorizontalPromptAlignProperty); }
			set { SetValue(HorizontalPromptAlignProperty, value); }
		}

        public static readonly DependencyProperty TextSizeProperty =
			DependencyProperty.Register("TextSize", typeof(int),
				typeof(WatermarkBox), new PropertyMetadata(10));
		public int TextSize
		{
			get { return (int)GetValue(TextSizeProperty); }
			set { SetValue(TextSizeProperty, value); }
		}

		public static readonly DependencyProperty MaxLengthProperty =
			DependencyProperty.Register("MaxLength", typeof(int),
				typeof(WatermarkBox), new PropertyMetadata(100));
		public int MaxLength
		{
			get { return (int)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value); }
		}

		public static readonly DependencyProperty BoxWidthProperty =
			DependencyProperty.Register("BoxWidth", typeof(double),
				typeof(WatermarkBox), new PropertyMetadata(100.0));
		public double BoxWidth
		{
			get { return (double)GetValue(BoxWidthProperty); }
			set { SetValue(BoxWidthProperty, value); }
		}

		public static readonly DependencyProperty BoxHeightProperty =
			DependencyProperty.Register("BoxHeight", typeof(double),
				typeof(WatermarkBox), new PropertyMetadata(20.0));
		public double BoxHeight
		{
			get { return (double)GetValue(BoxHeightProperty); }
			set { SetValue(BoxHeightProperty, value); }
		}

		public static readonly DependencyProperty IsPasswordProperty =
			DependencyProperty.Register("IsPassword", typeof(bool),
				typeof(WatermarkBox), new PropertyMetadata(false));
		public bool IsPassword
		{
			get { return (bool)GetValue(IsPasswordProperty); }
			set { SetValue(IsPasswordProperty, value); }
		}

		public static readonly DependencyProperty HighlightInputBordersProperty =
			DependencyProperty.Register("HighlightInputBorders", typeof(bool),
				typeof(WatermarkBox), new PropertyMetadata(true));
		public bool HighlightInputBorders
		{
			get { return (bool)GetValue(HighlightInputBordersProperty); }
			set { SetValue(HighlightInputBordersProperty, value); }
		}

        public static readonly DependencyProperty HighlightBordersProperty =
            DependencyProperty.Register("HighlightBorders", typeof(Thickness),
                typeof(WatermarkBox), new PropertyMetadata(new Thickness(1, 1, 1, 1)));
        public Thickness HighlightBorders
        {
            get { return (Thickness)GetValue(HighlightBordersProperty); }
            set { SetValue(HighlightBordersProperty, value); }
        }

        #endregion

        #region --- Custom EVENTS ---

        #region --- Custom TextChanged Event ---
        public static readonly RoutedEvent TextChangedEvent =
		  EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble,
			typeof(TextChangedEventArgs), typeof(WatermarkBox));

		////the event handler delegate
		public delegate void NewColorCustomEventHandler(object sender, TextChangedEventArgs e);

		public event RoutedEventHandler TextChanged
		{
			add { AddHandler(TextChangedEvent, value); }
			remove { RemoveHandler(TextChangedEvent, value); }
		}

		private void RaiseTextChangedEvent()
		{
			TextChangedEventArgs newEventArgs;
			if (this.IsPassword)
				newEventArgs = new TextChangedEventArgs(this.passwordUserEntry.Password);
			else
				newEventArgs = new TextChangedEventArgs(this.txtUserEntry.Text);
			newEventArgs.RoutedEvent = WatermarkBox.TextChangedEvent;
			RaiseEvent(newEventArgs);
		}

		/// <summary>
		/// ColorRoutedEventArgs : a custom event argument class
		/// </summary>
		public class TextChangedEventArgs: RoutedEventArgs
		{
			/// <summary>
			/// Gets the stored text
			/// </summary>
			public string Text { get; } = "";
			
			#region Constructor
			/// <summary>
			/// Constructs a new TextChangedEventArgs object
			/// using the parameters provided
			/// </summary>
			/// <param name="text">the TextBox or PasswordBox text</param>
			public TextChangedEventArgs(string text)
			{
				this.Text = text;
			}
			#endregion
		}

		#endregion

		#endregion

		public WatermarkBox()
        {
            InitializeComponent();
        }

		public void SetWatermark(string prompt)
        {
            this.Block.Text = prompt;
        }

		public void SetText(string text)
		{
			this.txtUserEntry.Text = text;
		}

		public string GetText()
        {
			string text;
			if (IsPassword)
			{
				text = this.passwordUserEntry.Password;
				this.passwordUserEntry.Password = "";
			}
			else
			{
				text = this.txtUserEntry.Text;
				this.txtUserEntry.Text = "";
			}
            return text;
        }

		public string PeekText()
		{
			string text;
			if (IsPassword)
			{
				text = this.passwordUserEntry.Password;
			}
			else
			{
				text = this.txtUserEntry.Text;
			}
			return text;
		}

        public void SetFontSize(int point)
        {
            this.Block.FontSize = point;
            this.txtUserEntry.FontSize = point + 2;
			this.passwordUserEntry.FontSize = point + 2;
        }

        public void SetInputLength(int len)
        {
            this.txtUserEntry.MaxLength = len;
        }

        public void SetBackColor(SolidColorBrush color)
        {
			this.Block.Background = color;
        }

        public void SetHorizontalTextAlignment(HorizontalAlignment align)
        {
            this.Block.HorizontalAlignment = align;
            this.txtUserEntry.HorizontalContentAlignment = align;
        }

        public void SetBorders(double[] size, Color color)
        {
            this.BlockBorder.BorderThickness = new Thickness(size[0], size[1], size[2], size[3]);
            this.BlockBorder.BorderBrush = new SolidColorBrush(color);
        }

		private void PasswordUserEntry_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (this.passwordUserEntry.Password.Length > 0)
				this.Block.Visibility = Visibility.Collapsed;
			else
				this.Block.Visibility = Visibility.Visible;

			RaiseTextChangedEvent();
		}

		private void TxtUserEntry_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			RaiseTextChangedEvent();
		}
	}
}
