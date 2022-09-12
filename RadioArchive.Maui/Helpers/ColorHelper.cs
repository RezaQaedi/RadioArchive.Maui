namespace RadioArchive.Maui
{
    internal static class ColorHelper
    {
        private readonly static Random _random = new();

        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        public static Color Lerp(this Color colour, Color to, float amount)
        {
            // start colours as lerp-able floats
            float sr = colour.Red, sg = colour.Green, sb = colour.Blue;

            // end colours as lerp-able floats
            float er = to.Red, eg = to.Green, eb = to.Blue;

            // lerp the colours to get the difference
            byte r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);

            // return the new colour
            return Color.FromRgb(r, g, b);
        }

        public static Color GetRandomColor()
        {
            return Color.FromRgb((byte)_random.Next(225), (byte)_random.Next(225), (byte)_random.Next(225));
        }
    }
}
