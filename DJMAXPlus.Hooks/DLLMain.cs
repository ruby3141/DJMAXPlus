// CA2255: The ModuleInitializer attribute should not be used in libraries
// This library should be injected into other assembly, so it's not the case.
#pragma warning disable CA2255

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using SharpDX.DXGI;

namespace DJMAXPlus.Hooks
{
    static class DLLMain
    {
        static private IHook<DX11Hook.Present>? presentHook;
        static private IHook<DX11Hook.ResizeBuffers>? resizeBuffersHook;

        [ModuleInitializer]
        static internal void ModuleInitializer()
        {
            DX11Hook.Init(ReloadedHooks.Instance);
            ReloadedHooks.Instance.CreateHook<DX11Hook.Present>(typeof(DLLMain), nameof(Present),
                DX11Hook.DXGIVTable![(int)DX11Hook.IDXGISwapChain.Present].FunctionPointer);
            ReloadedHooks.Instance.CreateHook<DX11Hook.ResizeBuffers>(typeof(DLLMain), nameof(ResizeBuffers),
                DX11Hook.DXGIVTable![(int)DX11Hook.IDXGISwapChain.ResizeBuffers].FunctionPointer);
        }

        static private unsafe IntPtr Present(IntPtr swapChainPtr, int syncInterval, PresentFlags flags)
        {
            nint value = presentHook!.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);

            return value;
        }

        static private unsafe IntPtr ResizeBuffers(IntPtr swapchainPtr, uint bufferCount, uint width, uint height, Format newFormat, uint swapchainFlags)
        {
            nint value = resizeBuffersHook!.OriginalFunction.Value.Invoke(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);

            return value;
        }
    }
}
