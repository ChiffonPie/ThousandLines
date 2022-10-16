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

        PLAY        =  3, // ����

        MOVE        =  4, // �۾��� ���� ��ҷ� �̵�
        WAIT        =  5, // �۾� �Ϸ� ��� ����

        IN          =  6, // �ӽ� ��ġ
        OUT         =  7, // �ӽ� ���� 
        REPOSITION  =  8, // �ӽ� ���� �缳��

        STOP        =  9, // ����
    }

    public enum ProcessingType
    {
        NULL    = -1,
        PRESS   =  0,
        WELDING =  1,
        SOAK    =  2,
    }
}