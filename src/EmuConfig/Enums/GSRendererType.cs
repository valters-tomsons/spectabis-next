namespace EmuConfig.Enums
{
    // https://github.com/PCSX2/pcsx2/blob/master/plugins/GSdx/GS.h#L231
    public enum GSRendererType
    {
        Automatic = -1,

        DX1011_HW = 3,
        DX1011_SW,

        Null = 11,

        OGL_HW = 12,
        OGL_SW = 13,

        DX1011_OpenCL = 15,
        OGL_OpenCL = 17,
    }
}
