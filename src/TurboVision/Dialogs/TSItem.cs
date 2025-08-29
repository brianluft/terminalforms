using System.Runtime.InteropServices;

namespace TurboVision.Dialogs;

/// <summary>
/// Represents a string item in a linked list structure used by TCluster controls.
/// </summary>
public unsafe partial class TSItem(void* ptr, bool owned, bool placement)
    : NativeObject<TSItem>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TSItem_placementSize) { }

        public unsafe void* PlacementNew2(byte* ptr, string aValue, void* aNext)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TSItem_placementNew2(ptr, aValue, aNext));
            return ptr;
        }

        public static unsafe void* New2(string aValue, void* aNext)
        {
            TurboVisionException.Check(NativeMethods.TV_TSItem_new2(out var ptr, aValue, aNext));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    /// <summary>
    /// Creates a new TSItem with the specified value and next item using placement new.
    /// </summary>
    /// <param name="placement">Memory location for placement allocation</param>
    /// <param name="aValue">The string value for this item</param>
    /// <param name="aNext">The next item in the list (may be null)</param>
    public TSItem(byte* placement, string aValue, TSItem? aNext)
        : this(CreateWithPlacement(placement, aValue, aNext), owned: true, placement: true) { }

    /// <summary>
    /// Creates a new TSItem with the specified value and next item.
    /// </summary>
    /// <param name="aValue">The string value for this item</param>
    /// <param name="aNext">The next item in the list (may be null)</param>
    public TSItem(string aValue, TSItem? aNext)
        : this(CreateNew(aValue, aNext), owned: true, placement: false) { }

    private static unsafe void* CreateWithPlacement(byte* placement, string aValue, TSItem? aNext)
    {
        void* nextPtr = aNext != null ? aNext.Ptr : null;
        return Factory.Instance.PlacementNew2(placement, aValue, nextPtr);
    }

    private static unsafe void* CreateNew(string aValue, TSItem? aNext)
    {
        void* nextPtr = aNext != null ? aNext.Ptr : null;
        return Factory.New2(aValue, nextPtr);
    }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TSItem_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TSItem_delete(Ptr));
    }

    protected override bool EqualsCore(TSItem other)
    {
        TurboVisionException.Check(NativeMethods.TV_TSItem_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TSItem_hash(Ptr, out var hash));
        return hash;
    }

    /// <summary>
    /// Gets or sets the string value of this item.
    /// </summary>
    public string Value
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TSItem_get_value(Ptr, out var valuePtr));
            return Marshal.PtrToStringUTF8((nint)valuePtr) ?? string.Empty;
        }
        set { TurboVisionException.Check(NativeMethods.TV_TSItem_set_value(Ptr, value)); }
    }

    /// <summary>
    /// Gets or sets the next item in the linked list.
    /// </summary>
    public TSItem? Next
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TSItem_get_next(Ptr, out var nextPtr));
            return nextPtr != null ? new TSItem(nextPtr, owned: false, placement: false) : null;
        }
        set
        {
            void* nextPtr = value != null ? value.Ptr : null;
            TurboVisionException.Check(NativeMethods.TV_TSItem_set_next(Ptr, nextPtr));
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TSItem_placementNew2(void* self, string aValue, void* aNext);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TSItem_new2(out void* @out, string aValue, void* aNext);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_get_value(void* self, out byte* result);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TSItem_set_value(void* self, string value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_get_next(void* self, out void* result);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSItem_set_next(void* self, void* value);
    }
}
