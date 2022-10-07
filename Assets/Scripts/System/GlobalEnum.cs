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

    public enum BaseMachineState
    {
        NULL        = -1,
        INITIALIZE  =  0,
        READY       =  1,
        CREATE      =  2,
        WAIT        =  3,
        MOVE        =  4,
    }

    public enum MachineLineState
    {
        NULL       = -1,
        INITIALIZE =  0,
        PROCESSING =  1,
        WAIT       =  2,
        MOVE       =  3,
        CHANGE     =  4,
    }
}