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
        NULL   = -1,
        READY  =  0,
        CREATE =  1,
        WAIT   =  2,
        MOVE   =  3,
    }


    public enum MachineLineState
    {
        NULL       = -1,
        PROCESSING =  0,
        WAIT       =  1,
        MOVE       =  2,
        CHANGE     =  3,
    }
}