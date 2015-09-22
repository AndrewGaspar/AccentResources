using System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace AccentResources
{
    public sealed class AccentProvider : DependencyObject
    {
        private UISettings UISettings = null;

        private static byte Average(byte a, byte b)
        {
            return (byte)((a + b) / 2);
        }

        private static Color MixColors(Color a, Color b)
        {
            return new Color
            {
                A = 0xFF,
                R = Average(a.R, b.R),
                G = Average(a.G, b.G),
                B = Average(a.B, b.B),
            };
        }

        public AccentProvider()
        {
            try
            {
                UISettings = new UISettings();

                UISettings.ColorValuesChanged += UISettings_ColorValuesChanged;

                UpdateColors();
            }
            // Due to a bug in the 10240 build of Windows 10 Mobile, the
            // accent color cannot be acquired using this method.
            // Until this is fixed, we'll have to fall back to a default
            // value.
            // This bug should be fixed by Windows 10 Mobile's release.
            catch (InvalidCastException)
            {
                UISettings = null;

                var theme = Application.Current == null ? ApplicationTheme.Dark : Application.Current.RequestedTheme;

                if (theme == ApplicationTheme.Light)
                {
                    BackgroundColor = Colors.White;
                    ForegroundColor = Colors.Black;
                }
                else if (theme == ApplicationTheme.Dark)
                {
                    BackgroundColor = Colors.Black;
                    ForegroundColor = Colors.White;
                }

                object accentColorObj;

                if (Application.Current != null &&
                    Application.Current.Resources.TryGetValue("SystemAccentColor", out accentColorObj) &&
                    accentColorObj is Color)
                {
                    AccentColor = (Color)accentColorObj;
                }
                else
                {
                    AccentColor = Colors.BlueViolet;
                }

                // This is almost certainly wrong - but it's a decent stop gap
                AccentDark3Color = MixColors(Colors.Black, AccentColor);
                AccentDark2Color = MixColors(AccentDark3Color, AccentColor);
                AccentDark1Color = MixColors(AccentDark2Color, AccentColor);

                AccentLight3Color = MixColors(Colors.White, AccentColor);
                AccentLight2Color = MixColors(AccentLight3Color, AccentColor);
                AccentLight1Color = MixColors(AccentLight2Color, AccentColor);

                ComplementColor = Color.FromArgb(0xFF,
                    (byte)(0xFF - AccentColor.R),
                    (byte)(0xFF - AccentColor.G),
                    (byte)(0xFF - AccentColor.B));
            }
        }

        private void UISettings_ColorValuesChanged(UISettings sender, object args)
        {
            UpdateColors();
        }

        private void UpdateColors()
        {
            if (UISettings == null) return;

            BackgroundColor = UISettings.GetColorValue(UIColorType.Background);
            ForegroundColor = UISettings.GetColorValue(UIColorType.Foreground);
            AccentDark3Color = UISettings.GetColorValue(UIColorType.AccentDark3);
            AccentDark2Color = UISettings.GetColorValue(UIColorType.AccentDark2);
            AccentDark1Color = UISettings.GetColorValue(UIColorType.AccentDark1);
            AccentColor = UISettings.GetColorValue(UIColorType.Accent);
            AccentLight1Color = UISettings.GetColorValue(UIColorType.AccentLight1);
            AccentLight2Color = UISettings.GetColorValue(UIColorType.AccentLight2);
            AccentLight3Color = UISettings.GetColorValue(UIColorType.AccentLight3);

            try
            {
                ComplementColor = UISettings.GetColorValue(UIColorType.Complement);
            }
            // There's a bug in my build (10540) that throws ArgumentException for UIColorType.Complement
            // If this gets hit, just invert the color. This is not actually a good option.
            catch (ArgumentException)
            {
                ComplementColor = Color.FromArgb(0xFF,
                    (byte)(0xFF - AccentColor.R),
                    (byte)(0xFF - AccentColor.G),
                    (byte)(0xFF - AccentColor.B));
            }
        }

        #region Color Dependency Properties

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static DependencyProperty BackgroundColorProperty { get; } =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundColor.  This enables animation, styling, binding, etc...
        public static DependencyProperty ForegroundColorProperty { get; } =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentDark3Color
        {
            get { return (Color)GetValue(AccentDark3ColorProperty); }
            set { SetValue(AccentDark3ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentDark3Color.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentDark3ColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentDark3Color), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentDark2Color
        {
            get { return (Color)GetValue(AccentDark2ColorProperty); }
            set { SetValue(AccentDark2ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentDark2Color.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentDark2ColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentDark2Color), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentDark1Color
        {
            get { return (Color)GetValue(AccentDark1ColorProperty); }
            set { SetValue(AccentDark1ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentDark1Color.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentDark1ColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentDark1Color), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentColor.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentColor), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentLight1Color
        {
            get { return (Color)GetValue(AccentLight1ColorProperty); }
            set { SetValue(AccentLight1ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentLight1Color.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentLight1ColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentLight1Color), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentLight2Color
        {
            get { return (Color)GetValue(AccentLight2ColorProperty); }
            set { SetValue(AccentLight2ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentLight2Color.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentLight2ColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentLight2Color), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color AccentLight3Color
        {
            get { return (Color)GetValue(AccentLight3ColorProperty); }
            set { SetValue(AccentLight3ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentLight3Color.  This enables animation, styling, binding, etc...
        public static DependencyProperty AccentLight3ColorProperty { get; } =
            DependencyProperty.Register(nameof(AccentLight3Color), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        public Color ComplementColor
        {
            get { return (Color)GetValue(ComplementColorProperty); }
            set { SetValue(ComplementColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ComplementaryColor.  This enables animation, styling, binding, etc...
        public static DependencyProperty ComplementColorProperty { get; } =
            DependencyProperty.Register(nameof(ComplementColor), typeof(Color), typeof(AccentProvider), new PropertyMetadata(0));

        #endregion Color Dependency Properties
    }
}