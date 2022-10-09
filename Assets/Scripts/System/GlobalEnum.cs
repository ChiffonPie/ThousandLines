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
        NULL        = -1, // 정상적이지 않은 상태
        INITIALIZE  =  0, // 초기화
        READY       =  1, // 준비 완료

        PLAY        =  2, // 가동

        WAIT        =  3, // 작업 완료 대기 상태
        MOVE        =  4, // 작업물 다음 장소로 이동

        CHANGE      =  5, // 머신 위치(순서) 이동 
    }

    public enum MachineAbility
    {
        NULL    = -1,
        PRESS   =  0,
        WELDING =  1,
        SOAK    =  2,
    }
}