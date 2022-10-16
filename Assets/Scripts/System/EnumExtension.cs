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

            UnityEngine.Debug.LogError("���ǵ��� ���� ó�� ����� �ֽ��ϴ�.");
            return ProcessingType.NULL;
        }
    }
}