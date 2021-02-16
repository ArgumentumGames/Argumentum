using System;
using System.Drawing;

namespace Argumentum.AssetConverter.Mindmapper
{

    public static class ColorExtensions
    {
        static Random randomizer = new Random();
        public static Color GetContrast(this Color source, bool preserveOpacity)
        {
            Color inputColor = source;
            //if RGB values are close to each other by a diff less than 10%, then if RGB values are lighter side, decrease the blue by 50% (eventually it will increase in conversion below), if RBB values are on darker side, decrease yellow by about 50% (it will increase in conversion)
            byte avgColorValue = (byte)((source.R + source.G + source.B) / 3);
            int diff_r = Math.Abs(source.R - avgColorValue);
            int diff_g = Math.Abs(source.G - avgColorValue);
            int diff_b = Math.Abs(source.B - avgColorValue);
            if (diff_r < 20 && diff_g < 20 && diff_b < 20) //The color is a shade of gray
            {
                if (avgColorValue < 123) //color is dark
                {
                    inputColor = Color.FromArgb(50, 230, 220);
                    //inputColor.B = 220;
                    //inputColor.G = 230;
                    //inputColor.R = 50;
                }
                else
                {
                    inputColor = Color.FromArgb(255, 255, 50);
                    //inputColor.R = 255;
                    //inputColor.G = 255;
                    //inputColor.B = 50;
                }
            }
            byte sourceAlphaValue = source.A;
            if (!preserveOpacity)
            {
                sourceAlphaValue = Math.Max(source.A, (byte)127); //We don't want contrast color to be more than 50% transparent ever.
            }
            RGB rgb = new RGB { R = inputColor.R, G = inputColor.G, B = inputColor.B };
            HSB hsb = ConvertToHSB(rgb);
            hsb.H = hsb.H < 180? hsb.H + 180 : hsb.H - 180;
            //hsb.B = isColorDark ? 240 : 50; //Added to create dark on light, and light on dark
            rgb = ConvertToRGB(hsb);
            //return new Color { A = sourceAlphaValue, R = rgb.R, G = (byte)rgb.G, B = (byte)rgb.B };
            return Color.FromArgb(sourceAlphaValue, (int) rgb.R, (int) rgb.G, (int)rgb.B );
        }
        internal static RGB ConvertToRGB(HSB hsb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            double chroma = hsb.S * hsb.B;
            double hue2 = hsb.H / 60;
            double x = chroma * (1 - Math.Abs(hue2 % 2 - 1));
            double r1 = 0d;
            double g1 = 0d;
            double b1 = 0d;
            if (hue2 >= 0 && hue2 < 1)
            {
                r1 = chroma;
                g1 = x;
            }
            else if (hue2 >= 1 && hue2 < 2)
            {
                r1 = x;
                g1 = chroma;
            }
            else if (hue2 >= 2 && hue2 < 3)
            {
                g1 = chroma;
                b1 = x;
            }
            else if (hue2 >= 3 && hue2 < 4)
            {
                g1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 4 && hue2 < 5)
            {
                r1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 5 && hue2 <= 6)
            {
                r1 = chroma;
                b1 = x;
            }
            double m = hsb.B - chroma;
            return new RGB()
            {
                R = r1 + m,
                G = g1 + m,
                B = b1 + m
            };
        }
        internal static HSB ConvertToHSB(RGB rgb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            double r = rgb.R;
            double g = rgb.G;
            double b = rgb.B;

            double max = Max(r, g, b);
            double min = Min(r, g, b);
            double chroma = max - min;
            double hue2 = 0d;
            if (chroma != 0)
            {
                if (max == r)
                {
                    hue2 = (g - b) / chroma;
                }
                else if (max == g)
                {
                    hue2 = (b - r) / chroma + 2;
                }
                else
                {
                    hue2 = (r - g) / chroma + 4;
                }
            }
            double hue = hue2 * 60;
            if (hue < 0)
            {
                hue += 360;
            }
            double brightness = max;
            double saturation = 0;
            if (chroma != 0)
            {
                saturation = chroma / brightness;
            }
            return new HSB()
            {
                H = hue,
                S = saturation,
                B = brightness
            };
        }
        private static double Max(double d1, double d2, double d3)
        {
            if (d1 > d2)
            {
                return Math.Max(d1, d3);
            }
            return Math.Max(d2, d3);
        }
        private static double Min(double d1, double d2, double d3)
        {
            if (d1 < d2)
            {
                return Math.Min(d1, d3);
            }
            return Math.Min(d2, d3);
        }
        internal struct RGB
        {
            internal double R;
            internal double G;
            internal double B;
        }
        internal struct HSB
        {
            internal double H;
            internal double S;
            internal double B;
        }
    }


    public struct HLSColor
    {
        private const int ShadowAdj = -333;
        private const int HilightAdj = 500;
        private const int WatermarkAdj = -50;
        private const int Range = 240;
        private const int HLSMax = 240;
        private const int RGBMax = 255;
        private const int Undefined = 160;
        private int hue;
        private int saturation;
        private int luminosity;
        private bool isSystemColors_Control;


        public static Color GetLighterColor(Color baseColor)
        {
            var toReturn = new HLSColor(baseColor).Lighter(0.999f);
            toReturn = Color.FromArgb( 10, toReturn.R, toReturn.G, toReturn.B);
            return toReturn;
        }

        public static string GetLighterColor(string baseColor)
        {
            return ColorTranslator.ToHtml(GetLighterColor(ColorTranslator.FromHtml(baseColor)));
        }


        public static Color GetDarkerColor(Color baseColor)
        {
            return new HLSColor(baseColor).Darker(0.2f);
        }

        public static string GetDarkerColor(string baseColor)
        {
            return ColorTranslator.ToHtml(GetDarkerColor(ColorTranslator.FromHtml(baseColor)));
        }


        public HLSColor(Color color)
        {
            this.isSystemColors_Control = color.ToKnownColor() == SystemColors.Control.ToKnownColor();
            int r = (int)color.R;
            int g = (int)color.G;
            int b = (int)color.B;
            int num1 = Math.Max(Math.Max(r, g), b);
            int num2 = Math.Min(Math.Min(r, g), b);
            int num3 = num1 + num2;
            this.luminosity = (num3 * 240 + (int)byte.MaxValue) / 510;
            int num4 = num1 - num2;
            if (num4 == 0)
            {
                this.saturation = 0;
                this.hue = 160;
            }
            else
            {
                this.saturation = this.luminosity > 120 ? (num4 * 240 + (510 - num3) / 2) / (510 - num3) : (num4 * 240 + num3 / 2) / num3;
                int num5 = ((num1 - r) * 40 + num4 / 2) / num4;
                int num6 = ((num1 - g) * 40 + num4 / 2) / num4;
                int num7 = ((num1 - b) * 40 + num4 / 2) / num4;
                this.hue = r != num1 ? (g != num1 ? 160 + num6 - num5 : 80 + num5 - num7) : num7 - num6;
                if (this.hue < 0)
                    this.hue += 240;
                if (this.hue <= 240)
                    return;
                this.hue -= 240;
            }
        }

        public int Luminosity
        {
            get
            {
                return this.luminosity;
            }
        }

        public Color Darker(float percDarker)
        {
            if (this.isSystemColors_Control)
            {
                if ((double)percDarker == 0.0)
                    return SystemColors.ControlDark;
                if ((double)percDarker == 1.0)
                    return SystemColors.ControlDarkDark;
                Color controlDark = SystemColors.ControlDark;
                Color controlDarkDark = SystemColors.ControlDarkDark;
                int num1 = (int)controlDark.R - (int)controlDarkDark.R;
                int num2 = (int)controlDark.G - (int)controlDarkDark.G;
                int num3 = (int)controlDark.B - (int)controlDarkDark.B;
                return Color.FromArgb((int)(byte)((uint)controlDark.R - (uint)(byte)((double)num1 * (double)percDarker)), (int)(byte)((uint)controlDark.G - (uint)(byte)((double)num2 * (double)percDarker)), (int)(byte)((uint)controlDark.B - (uint)(byte)((double)num3 * (double)percDarker)));
            }
            int num4 = 0;
            int num5 = this.NewLuma(-333, true);
            return this.ColorFromHLS(this.hue, num5 - (int)((double)(num5 - num4) * (double)percDarker), this.saturation);
        }

        public override bool Equals(object o)
        {
            return o is HLSColor hlsColor && this.hue == hlsColor.hue && (this.saturation == hlsColor.saturation && this.luminosity == hlsColor.luminosity) && this.isSystemColors_Control == hlsColor.isSystemColors_Control;
        }

        public static bool operator == (HLSColor a, HLSColor b)
        {
            return a.Equals((object)b);
        }

        public static bool operator != (HLSColor a, HLSColor b)
        {
            return !a.Equals((object)b);
        }

        public override int GetHashCode()
        {
            return this.hue << 6 | this.saturation << 2 | this.luminosity;
        }

        public Color Lighter(float percLighter)
        {
            if (this.isSystemColors_Control)
            {
                if ((double)percLighter == 0.0)
                    return SystemColors.ControlLight;
                if ((double)percLighter == 1.0)
                    return SystemColors.ControlLightLight;
                Color controlLight = SystemColors.ControlLight;
                Color controlLightLight = SystemColors.ControlLightLight;
                int num1 = (int)controlLight.R - (int)controlLightLight.R;
                int num2 = (int)controlLight.G - (int)controlLightLight.G;
                int num3 = (int)controlLight.B - (int)controlLightLight.B;
                return Color.FromArgb((int)(byte)((uint)controlLight.R - (uint)(byte)((double)num1 * (double)percLighter)), (int)(byte)((uint)controlLight.G - (uint)(byte)((double)num2 * (double)percLighter)), (int)(byte)((uint)controlLight.B - (uint)(byte)((double)num3 * (double)percLighter)));
            }
            int luminosity = this.luminosity;
            int num = this.NewLuma(500, true);
            return this.ColorFromHLS(this.hue, luminosity + (int)((double)(num - luminosity) * (double)percLighter), this.saturation);
        }

        private int NewLuma(int n, bool scale)
        {
            return this.NewLuma(this.luminosity, n, scale);
        }

        private int NewLuma(int luminosity, int n, bool scale)
        {
            if (n == 0)
                return luminosity;
            if (scale)
                return n > 0 ? (int)(((long)(luminosity * (1000 - n)) + 241L * (long)n) / 1000L) : luminosity * (n + 1000) / 1000;
            int num = luminosity + (int)((long)n * 240L / 1000L);
            if (num < 0)
                num = 0;
            if (num > 240)
                num = 240;
            return num;
        }

        private Color ColorFromHLS(int hue, int luminosity, int saturation)
        {
            byte num1;
            byte num2;
            byte num3;
            if (saturation == 0)
            {
                int num4;
                num1 = (byte)(num4 = (int)(byte)(luminosity * (int)byte.MaxValue / 240));
                num2 = (byte)num4;
                num3 = (byte)num4;
                if (hue == 160)
                    ;
            }
            else
            {
                int n2 = luminosity > 120 ? luminosity + saturation - (luminosity * saturation + 120) / 240 : (luminosity * (240 + saturation) + 120) / 240;
                int n1 = 2 * luminosity - n2;
                num3 = (byte)((this.HueToRGB(n1, n2, hue + 80) * (int)byte.MaxValue + 120) / 240);
                num2 = (byte)((this.HueToRGB(n1, n2, hue) * (int)byte.MaxValue + 120) / 240);
                num1 = (byte)((this.HueToRGB(n1, n2, hue - 80) * (int)byte.MaxValue + 120) / 240);
            }
            return Color.FromArgb((int)num3, (int)num2, (int)num1);
        }

        private int HueToRGB(int n1, int n2, int hue)
        {
            if (hue < 0)
                hue += 240;
            if (hue > 240)
                hue -= 240;
            if (hue < 40)
                return n1 + ((n2 - n1) * hue + 20) / 40;
            if (hue < 120)
                return n2;
            return hue < 160 ? n1 + ((n2 - n1) * (160 - hue) + 20) / 40 : n1;
        }
    }
}