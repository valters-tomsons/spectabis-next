namespace EmuConfig.Enums
{
    // https://github.com/PCSX2/pcsx2/blob/master/plugins/GSdx/GS.h#L231
    public enum GSRendererType
    {
        Undefined = -1,

        DX1011_HW = 3,
        DX1011_SW,

        Null = 11,

        OGL_HW,
        OGL_SW,

        DX1011_OpenCL = 15,
        OGL_OpenCL = 17,
    }
}
