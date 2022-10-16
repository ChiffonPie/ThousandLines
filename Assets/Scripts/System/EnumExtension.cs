using ThousandLines;

namespace System.Collections.Generic
{
    public static class EnumExtension
    {
        public static ProcessingType ProsseingStringToEnum(string value)
        {
            switch (value)
            {
                case "Press"  : return ProcessingType.PRESS;
                case "Welding": return ProcessingType.WELDING;
                case "Soak"   : return ProcessingType.SOAK;
            }

            UnityEngine.Debug.LogError("정의되지 않은 처리 방법이 있습니다.");
            return ProcessingType.NULL;
        }
    }
}