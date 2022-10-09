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

            UnityEngine.Debug.LogError("���ǵ��� ���� ó�� ����� �ֽ��ϴ�.");
            return MachineAbility.NULL;
        }
    }
}