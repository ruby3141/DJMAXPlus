// Original code from github/Sewer56/Reloaded.Imgui.Hook, which is provided under MIT License.

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Hooks.Definitions.X64;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using CallingConventions = Reloaded.Hooks.Definitions.X86.CallingConventions;
using Device = SharpDX.Direct3D11.Device;

namespace DJMAXPlus.Hooks
{
    /// <summary>
    /// Provides access to DirectX 11 functions.
    /// </summary>
    public static class DX11Hook
    {
        /// <summary>
        /// Contains the DX11 Device VTable.
        /// </summary>
        public static IVirtualFunctionTable? VTable { get; private set; }

        /// <summary>
        /// Contains the DX11 DXGI Swapchain VTable.
        /// </summary>
        public static IVirtualFunctionTable? DXGIVTable { get; private set; }

        public static void Init(IReloadedHooks Hooks)
        {
            // Define
            Device dx11Device;
            SwapChain dxgiSwapChain;
            var renderForm = new Form();

            // Get Table
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, GetSwapChainDescription(renderForm.Handle), out dx11Device, out dxgiSwapChain);
            VTable = Hooks.VirtualFunctionTableFromObject(dx11Device.NativePointer, Enum.GetNames(typeof(ID3D11Device)).Length);
            DXGIVTable = Hooks.VirtualFunctionTableFromObject(dxgiSwapChain.NativePointer, Enum.GetNames(typeof(IDXGISwapChain)).Length);

            // Cleanup
            dxgiSwapChain.Dispose();
            dx11Device.Dispose();
            renderForm.Dispose();
        }

        private static SwapChainDescription GetSwapChainDescription(nint formHandle)
        {
            return new SwapChainDescription()
            {
                BufferCount = 1,
                IsWindowed = true,
                ModeDescription = new ModeDescription(640, 480, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = formHandle,
                SampleDescription = new SampleDescription(1, 0),
            };
        }

        [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
        public struct Present { public FuncPtr<nint, int, PresentFlags, nint> Value; }

        [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
        public struct ResizeBuffers { public FuncPtr<nint, uint, uint, uint, Format, uint, nint> Value; }

        /// <summary>
        /// Contains a full list of ID3D11Device functions to be used alongside
        /// <see cref="DX11Hook"/> as an indexer into the DirectX Virtual Function Table entries.
        /// </summary>
        public enum ID3D11Device
        {
            // IUnknown
            QueryInterface = 0,
            AddRef = 1,
            Release = 2,

            // ID3D11Device
            CreateBuffer = 3,
            CreateTexture1D = 4,
            CreateTexture2D = 5,
            CreateTexture3D = 6,
            CreateShaderResourceView = 7,
            CreateUnorderedAccessView = 8,
            CreateRenderTargetView = 9,
            CreateDepthStencilView = 10,
            CreateInputLayout = 11,
            CreateVertexShader = 12,
            CreateGeometryShader = 13,
            CreateGeometryShaderWithStreamOutput = 14,
            CreatePixelShader = 15,
            CreateHullShader = 16,
            CreateDomainShader = 17,
            CreateComputeShader = 18,
            CreateClassLinkage = 19,
            CreateBlendState = 20,
            CreateDepthStencilState = 21,
            CreateRasterizerState = 22,
            CreateSamplerState = 23,
            CreateQuery = 24,
            CreatePredicate = 25,
            CreateCounter = 26,
            CreateDeferredContext = 27,
            OpenSharedResource = 28,
            CheckFormatSupport = 29,
            CheckMultisampleQualityLevels = 30,
            CheckCounterInfo = 31,
            CheckCounter = 32,
            CheckFeatureSupport = 33,
            GetPrivateData = 34,
            SetPrivateData = 35,
            SetPrivateDataInterface = 36,
            GetFeatureLevel = 37,
            GetCreationFlags = 38,
            GetDeviceRemovedReason = 39,
            GetImmediateContext = 40,
            SetExceptionMode = 41,
            GetExceptionMode = 42,
        }

        /// <summary>
        /// Contains a full list of IDXGISwapChain functions to be used alongside
        /// <see cref="DX11Hook"/> as an indexer into the SwapChain Virtual Function Table entries.
        /// </summary>
        public enum IDXGISwapChain
        {
            // IUnknown
            QueryInterface = 0,
            AddRef = 1,
            Release = 2,

            // IDXGIObject
            SetPrivateData = 3,
            SetPrivateDataInterface = 4,
            GetPrivateData = 5,
            GetParent = 6,

            // IDXGIDeviceSubObject
            GetDevice = 7,

            // IDXGISwapChain
            Present = 8,
            GetBuffer = 9,
            SetFullscreenState = 10,
            GetFullscreenState = 11,
            GetDesc = 12,
            ResizeBuffers = 13,
            ResizeTarget = 14,
            GetContainingOutput = 15,
            GetFrameStatistics = 16,
            GetLastPresentCount = 17,
        }
    }
}
