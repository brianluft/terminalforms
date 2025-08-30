using System.Runtime.InteropServices;

namespace TurboVision.System;

public unsafe partial class TPMRegs(void* ptr, bool owned, bool placement)
    : NativeObject<TPMRegs>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TPMRegs_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TPMRegs(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TPMRegs()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TPMRegs_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TPMRegs_delete(Ptr));
    }

    protected override bool EqualsCore(TPMRegs other)
    {
        TurboVisionException.Check(NativeMethods.TV_TPMRegs_equals(Ptr, other.Ptr, out var result));
        return result;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TPMRegs_hash(Ptr, out var hash));
        return hash;
    }

    public uint Di
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_di(Ptr, out var di));
            return di;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_di(Ptr, value));
        }
    }

    public uint Si
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_si(Ptr, out var si));
            return si;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_si(Ptr, value));
        }
    }

    public uint Bp
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_bp(Ptr, out var bp));
            return bp;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_bp(Ptr, value));
        }
    }

    public uint Dummy
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_dummy(Ptr, out var dummy));
            return dummy;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_dummy(Ptr, value));
        }
    }

    public uint Bx
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_bx(Ptr, out var bx));
            return bx;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_bx(Ptr, value));
        }
    }

    public uint Dx
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_dx(Ptr, out var dx));
            return dx;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_dx(Ptr, value));
        }
    }

    public uint Cx
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_cx(Ptr, out var cx));
            return cx;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_cx(Ptr, value));
        }
    }

    public uint Ax
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_ax(Ptr, out var ax));
            return ax;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_ax(Ptr, value));
        }
    }

    public uint Flags
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_flags(Ptr, out var flags));
            return flags;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_flags(Ptr, value));
        }
    }

    public uint Es
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_es(Ptr, out var es));
            return es;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_es(Ptr, value));
        }
    }

    public uint Ds
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_ds(Ptr, out var ds));
            return ds;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_ds(Ptr, value));
        }
    }

    public uint Fs
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_fs(Ptr, out var fs));
            return fs;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_fs(Ptr, value));
        }
    }

    public uint Gs
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_gs(Ptr, out var gs));
            return gs;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_gs(Ptr, value));
        }
    }

    public uint Ip
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_ip(Ptr, out var ip));
            return ip;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_ip(Ptr, value));
        }
    }

    public uint Cs
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_cs(Ptr, out var cs));
            return cs;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_cs(Ptr, value));
        }
    }

    public uint Sp
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_sp(Ptr, out var sp));
            return sp;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_sp(Ptr, value));
        }
    }

    public uint Ss
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_get_ss(Ptr, out var ss));
            return ss;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPMRegs_set_ss(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_di(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_di(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_si(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_si(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_bp(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_bp(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_dummy(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_dummy(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_bx(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_bx(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_dx(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_dx(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_cx(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_cx(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_ax(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_ax(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_flags(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_flags(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_es(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_es(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_ds(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_ds(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_fs(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_fs(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_gs(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_gs(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_ip(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_ip(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_cs(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_cs(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_sp(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_sp(void* self, uint value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_get_ss(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPMRegs_set_ss(void* self, uint value);
    }
}
