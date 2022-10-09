using ThousandLines;

namespace System.Collections.Generic
{
    public static class EnumExtension
    {
        public static MachineAbility ProsseingStringToEnum(string value)
        {
            switch (value)
            {
                case "Press": return MachineAbility.PRESS;
                case "Welding": return MachineAbility.WELDING;
                case "Soak": return MachineAbility.SOAK;
            }

            UnityEngine.Debug.LogError("정의되지 않은 처리 방법이 있습니다.");
            return MachineAbility.NULL;
        }
    }
}