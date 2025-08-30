#include "TPMRegs.h"

TV_DEFAULT_CONSTRUCTOR(TPMRegs)
TV_BOILERPLATE_FUNCTIONS(TPMRegs)

// Get/Set functions for unsigned long members
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, di)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, si)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, bp)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, dummy)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, bx)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, dx)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, cx)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, ax)

// Get/Set functions for unsigned members
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, flags)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, es)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, ds)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, fs)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, gs)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, ip)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, cs)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, sp)
TV_GET_SET_PRIMITIVE(TPMRegs, uint32_t, ss)
