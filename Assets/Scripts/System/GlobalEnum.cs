namespace ThousandLines
{
    public enum MaterilObjectType 
    {
        Material_Null   = 0,
        Material_Base   = 1,
        Material_Iorn   = 2,
        Material_Bronze = 3,
        Material_Silver = 4,
        Material_Gold   = 5,
    }

    public enum MachineState
    {
        NULL        = -1, // ���������� ���� ����
        INITIALIZE  =  0, // �ʱ�ȭ
        READY       =  1, // �غ� �Ϸ�

        PLAY        =  2, // ����

        WAIT        =  3, // �۾� �Ϸ� ��� ����
        MOVE        =  4, // �۾��� ���� ��ҷ� �̵�

        CHANGE      =  5, // �ӽ� ��ġ(����) �̵� 
    }

    public enum MachineAbility
    {
        NULL    = -1,
        PRESS   =  0,
        WELDING =  1,
        SOAK    =  2,
    }
}