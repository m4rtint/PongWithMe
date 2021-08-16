using UnityEngine;

namespace PongWithMe
{
    public class ColorPalette
    {
        public static Color PastelGrey;
        public static Color PastelBlue;
        public static Color PastelRed;
        public static Color PastelYellow;
        public static Color PastelGreen;

        public ColorPalette()
        {            
            ColorUtility.TryParseHtmlString("#C4C4C4", out PastelGrey);
            ColorUtility.TryParseHtmlString("#54C1FE", out PastelBlue);
            ColorUtility.TryParseHtmlString("#EB5B5B", out PastelRed);
            ColorUtility.TryParseHtmlString("#FBFB95", out PastelYellow);
            ColorUtility.TryParseHtmlString("#77DD77", out PastelGreen);
        }
    }
}