using System.Runtime.InteropServices;

namespace TurboVision.Outlines;

/// <summary>
/// Represents a node in an outline structure, containing text and hierarchical relationships.
/// </summary>
public unsafe partial class TNode(void* ptr, bool owned, bool placement)
    : NativeObject<TNode>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TNode_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr, byte* aText)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TNode_placementNew(ptr, aText));
            return ptr;
        }

        public unsafe void* PlacementNew2(
            byte* ptr,
            byte* aText,
            void* aChildren,
            void* aNext,
            int initialState
        )
        {
            ptr = Align(ptr);
            TurboVisionException.Check(
                NativeMethods.TV_TNode_placementNew2(ptr, aText, aChildren, aNext, initialState)
            );
            return ptr;
        }

        public static unsafe void* New(byte* aText)
        {
            TurboVisionException.Check(NativeMethods.TV_TNode_new(out var ptr, aText));
            return ptr;
        }

        public static unsafe void* New2(byte* aText, void* aChildren, void* aNext, int initialState)
        {
            TurboVisionException.Check(
                NativeMethods.TV_TNode_new2(out var ptr, aText, aChildren, aNext, initialState)
            );
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    /// <summary>
    /// Creates a new TNode with the specified text using placement new.
    /// </summary>
    /// <param name="placement">Memory location for placement allocation</param>
    /// <param name="aText">The text for this node</param>
    public TNode(byte* placement, string aText)
        : this(CreateWithPlacement(placement, aText), owned: true, placement: true) { }

    /// <summary>
    /// Creates a new TNode with the specified text, children, next node, and initial state using placement new.
    /// </summary>
    /// <param name="placement">Memory location for placement allocation</param>
    /// <param name="aText">The text for this node</param>
    /// <param name="aChildren">The first child node (may be null)</param>
    /// <param name="aNext">The next sibling node (may be null)</param>
    /// <param name="initialState">The initial expanded state</param>
    public TNode(byte* placement, string aText, TNode? aChildren, TNode? aNext, bool initialState)
        : this(
            CreateWithPlacement2(placement, aText, aChildren, aNext, initialState),
            owned: true,
            placement: true
        ) { }

    /// <summary>
    /// Creates a new TNode with the specified text.
    /// </summary>
    /// <param name="aText">The text for this node</param>
    public TNode(string aText)
        : this(CreateNew(aText), owned: true, placement: false) { }

    /// <summary>
    /// Creates a new TNode with the specified text, children, next node, and initial state.
    /// </summary>
    /// <param name="aText">The text for this node</param>
    /// <param name="aChildren">The first child node (may be null)</param>
    /// <param name="aNext">The next sibling node (may be null)</param>
    /// <param name="initialState">The initial expanded state</param>
    public TNode(string aText, TNode? aChildren, TNode? aNext, bool initialState)
        : this(CreateNew2(aText, aChildren, aNext, initialState), owned: true, placement: false) { }

    private static unsafe void* CreateWithPlacement(byte* placement, string aText)
    {
        var textBytes = Global.UTF8Encoding.GetBytes(aText + "\0");
        fixed (byte* textPtr = textBytes)
        {
            return Factory.Instance.PlacementNew(placement, textPtr);
        }
    }

    private static unsafe void* CreateWithPlacement2(
        byte* placement,
        string aText,
        TNode? aChildren,
        TNode? aNext,
        bool initialState
    )
    {
        var textBytes = Global.UTF8Encoding.GetBytes(aText + "\0");
        fixed (byte* textPtr = textBytes)
        {
            void* childPtr = aChildren != null ? aChildren.Ptr : null;
            void* nextPtr = aNext != null ? aNext.Ptr : null;
            return Factory.Instance.PlacementNew2(
                placement,
                textPtr,
                childPtr,
                nextPtr,
                initialState ? 1 : 0
            );
        }
    }

    private static unsafe void* CreateNew(string aText)
    {
        var textBytes = Global.UTF8Encoding.GetBytes(aText + "\0");
        fixed (byte* textPtr = textBytes)
        {
            return Factory.New(textPtr);
        }
    }

    private static unsafe void* CreateNew2(
        string aText,
        TNode? aChildren,
        TNode? aNext,
        bool initialState
    )
    {
        var textBytes = Global.UTF8Encoding.GetBytes(aText + "\0");
        fixed (byte* textPtr = textBytes)
        {
            void* childPtr = aChildren != null ? aChildren.Ptr : null;
            void* nextPtr = aNext != null ? aNext.Ptr : null;
            return Factory.New2(textPtr, childPtr, nextPtr, initialState ? 1 : 0);
        }
    }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TNode_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TNode_delete(Ptr));
    }

    protected override bool EqualsCore(TNode other)
    {
        TurboVisionException.Check(NativeMethods.TV_TNode_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TNode_hash(Ptr, out var hash));
        return hash;
    }

    /// <summary>
    /// Gets or sets the next sibling node in the outline.
    /// </summary>
    public TNode? Next
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TNode_get_next(Ptr, out var result));
            return result != null ? new TNode(result, owned: false, placement: false) : null;
        }
        set
        {
            void* ptr = value != null ? value.Ptr : null;
            TurboVisionException.Check(NativeMethods.TV_TNode_set_next(Ptr, ptr));
        }
    }

    /// <summary>
    /// Gets or sets the text content of this node.
    /// </summary>
    public string Text
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TNode_get_text(Ptr, out var result));
            return result != null
                ? Marshal.PtrToStringUTF8((nint)result) ?? string.Empty
                : string.Empty;
        }
        set
        {
            var textBytes = Global.UTF8Encoding.GetBytes(value + "\0");
            fixed (byte* textPtr = textBytes)
            {
                TurboVisionException.Check(NativeMethods.TV_TNode_set_text(Ptr, textPtr));
            }
        }
    }

    /// <summary>
    /// Gets or sets the first child node in the outline.
    /// </summary>
    public TNode? ChildList
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TNode_get_childList(Ptr, out var result));
            return result != null ? new TNode(result, owned: false, placement: false) : null;
        }
        set
        {
            void* ptr = value != null ? value.Ptr : null;
            TurboVisionException.Check(NativeMethods.TV_TNode_set_childList(Ptr, ptr));
        }
    }

    /// <summary>
    /// Gets or sets whether this node is expanded in the outline view.
    /// </summary>
    public bool Expanded
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TNode_get_expanded(Ptr, out var result));
            return result;
        }
        set { TurboVisionException.Check(NativeMethods.TV_TNode_set_expanded(Ptr, value)); }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_placementNew(void* self, byte* aText);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_placementNew2(
            void* self,
            byte* aText,
            void* aChildren,
            void* aNext,
            int initialState
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_new(out void* @out, byte* aText);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_new2(
            out void* @out,
            byte* aText,
            void* aChildren,
            void* aNext,
            int initialState
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_get_next(void* self, out void* result);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_set_next(void* self, void* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_get_text(void* self, out byte* result);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_set_text(void* self, byte* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_get_childList(void* self, out void* result);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_set_childList(void* self, void* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_get_expanded(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool result
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TNode_set_expanded(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );
    }
}
