#pragma once

// NVTT 3 SDK path: C:\Program Files\NVIDIA Corporation\NVIDIA Texture Tools
// Include NVTT 3 headers - configure Additional Include Directories in project

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

// Forward declare NVTT types to avoid header dependency in managed code
namespace nvtt {
    struct CompressionOptions;
    struct OutputOptions;
    struct Context;
    struct Surface;
    struct OutputHandler;
    struct ErrorHandler;
    enum Format;
    enum Quality;
    enum WrapMode;
    enum TextureType;
    enum InputFormat;
    enum MipmapFilter;
    enum AlphaMode;
    enum RoundMode;
    enum Container;
    enum Error;
}

namespace CodeWalker {
namespace NVTT {

    public enum class Format
    {
        RGB = 0,
        RGBA = 0,
        DXT1 = 1,
        DXT1a = 2,
        DXT3 = 3,
        DXT5 = 4,
        DXT5n = 5,
        BC1 = 1,
        BC1a = 2,
        BC2 = 3,
        BC3 = 4,
        BC3n = 5,
        BC4 = 6,
        BC4S = 7,
        ATI2 = 8,
        BC5 = 9,
        BC5S = 10,
        DXT1n = 11,
        CTX1 = 12,
        BC6U = 13,
        BC6S = 14,
        BC7 = 15
    };

    public enum class Quality
    {
        Fastest = 0,
        Normal = 1,
        Production = 2,
        Highest = 3
    };

    public enum class WrapMode
    {
        Clamp = 0,
        Repeat = 1,
        Mirror = 2
    };

    public enum class TextureType
    {
        Texture2D = 0,
        Cube = 1,
        Texture3D = 2
    };

    public enum class InputFormat
    {
        BGRA_8UB = 0,
        BGRA_8SB = 1,
        RGBA_16F = 2,
        RGBA_32F = 3,
        R_32F = 4
    };

    public enum class MipmapFilter
    {
        Box = 0,
        Triangle = 1,
        Kaiser = 2,
        Mitchell = 3,
        Min = 4,
        Max = 5
    };

    public enum class AlphaMode
    {
        None = 0,
        Transparency = 1,
        Premultiplied = 2
    };

    public enum class RoundMode
    {
        None = 0,
        ToNextPowerOfTwo = 1,
        ToNearestPowerOfTwo = 2,
        ToPreviousPowerOfTwo = 3
    };

    public enum class Container
    {
        DDS = 0,
        DDS10 = 1
    };

    public enum class Error
    {
        None = 0,
        InvalidInput = 1,
        UnsupportedFeature = 2,
        CudaError = 3,
        FileOpen = 4,
        FileWrite = 5,
        UnsupportedOutputFormat = 6,
        OutOfHostMemory = 8,
        OutOfDeviceMemory = 9,
        OutputWrite = 10
    };

    // Forward declarations
    ref class Surface;
    ref class CompressionOptions;
    ref class OutputOptions;
    ref class Context;

    public interface class IOutputHandler
    {
        void BeginImage(int size, int width, int height, int depth, int face, int miplevel);
        bool WriteData(array<Byte>^ data, int offset, int size);
        void EndImage();
    };

    public interface class IUnsafeOutputHandler
    {
        void BeginImage(int size, int width, int height, int depth, int face, int miplevel);
        bool WriteData(IntPtr data, int size);
        void EndImage();
    };

    public interface class IErrorHandler
    {
        void HandleError(Error error);
    };

    public ref class CompressionOptions : public IDisposable
    {
    private:
        nvtt::CompressionOptions* m_pOptions;
        bool m_disposed;

    public:
        CompressionOptions();
        ~CompressionOptions();
        !CompressionOptions();

        void Reset();
        void SetFormat(Format format);
        void SetQuality(Quality quality);
        void SetColorWeights(float red, float green, float blue);
        void SetColorWeights(float red, float green, float blue, float alpha);
        void SetPixelFormat(unsigned int bitCount, unsigned int rmask, unsigned int gmask, unsigned int bmask, unsigned int amask);
        void SetPixelFormat(unsigned char rsize, unsigned char gsize, unsigned char bsize, unsigned char asize);
        void SetQuantization(bool colorDithering, bool alphaDithering, bool binaryAlpha);
        void SetQuantization(bool colorDithering, bool alphaDithering, bool binaryAlpha, int alphaThreshold);
        unsigned int GetD3D9Format();

        property nvtt::CompressionOptions* Native { nvtt::CompressionOptions* get() { return m_pOptions; } }
    };

    public ref class OutputOptions : public IDisposable
    {
    private:
        nvtt::OutputOptions* m_pOptions;
        nvtt::OutputHandler* m_pOutputHandler;
        nvtt::ErrorHandler* m_pErrorHandler;
        GCHandle m_outputHandlerHandle;
        GCHandle m_errorHandlerHandle;
        bool m_disposed;

    public:
        OutputOptions();
        ~OutputOptions();
        !OutputOptions();

        void Reset();
        void SetFileName(String^ fileName);
        void SetOutputHandler(IOutputHandler^ handler);
        void SetOutputHandler(IUnsafeOutputHandler^ handler);
        void SetErrorHandler(IErrorHandler^ handler);
        void SetOutputHeader(bool outputHeader);
        void SetContainer(Container container);
        void SetSrgbFlag(bool srgb);

        property nvtt::OutputOptions* Native { nvtt::OutputOptions* get() { return m_pOptions; } }
    };

    public ref class Surface : public IDisposable
    {
    private:
        nvtt::Surface* m_pSurface;
        bool m_disposed;

    public:
        Surface();
        ~Surface();
        !Surface();

        // Image loading
        bool Load(String^ fileName);
        bool Load(String^ fileName, [Out] bool% hasAlpha);
        bool LoadFromMemory(array<Byte>^ data);
        bool LoadFromMemory(array<Byte>^ data, [Out] bool% hasAlpha);

        // Set image data from raw pixels
        bool SetImage(int width, int height, int depth);
        bool SetImage(InputFormat format, int width, int height, int depth, IntPtr data);
        bool SetImage(InputFormat format, int width, int height, int depth, array<Byte>^ data);

        // Properties
        property bool IsNull { bool get(); }
        property int Width { int get(); }
        property int Height { int get(); }
        property int Depth { int get(); }
        property TextureType Type { TextureType get(); }
        property int MipmapCount { int get(); }

        // Image processing
        void SetWrapMode(WrapMode mode);
        void SetAlphaMode(AlphaMode mode);
        void SetNormalMap(bool isNormalMap);

        // Mipmap generation
        bool BuildNextMipmap(MipmapFilter filter);
        bool BuildNextMipmap(MipmapFilter filter, int minSize);
        bool CanMakeNextMipmap();
        bool CanMakeNextMipmap(int minSize);

        // Resize
        void Resize(int width, int height, int depth, MipmapFilter filter);
        void ResizeToMaxExtent(int maxExtent, RoundMode mode, MipmapFilter filter);

        // Color space
        void ToLinear(float gamma);
        void ToGamma(float gamma);
        void ToSrgb();
        void ToLinearFromSrgb();

        // Clone
        Surface^ Clone();

        property nvtt::Surface* Native { nvtt::Surface* get() { return m_pSurface; } }
    };

    public ref class MemoryOutputHandler : public IUnsafeOutputHandler
    {
    private:
        MemoryStream^ m_stream;
        int m_currentImageSize;

    public:
        MemoryOutputHandler();

        virtual void BeginImage(int size, int width, int height, int depth, int face, int miplevel);
        virtual bool WriteData(IntPtr data, int size);
        virtual void EndImage();

        array<Byte>^ GetData();
        void Reset();

        property int Length { int get() { return (int)m_stream->Length; } }
    };

    public ref class Context : public IDisposable
    {
    private:
        nvtt::Context* m_pContext;
        bool m_disposed;

    public:
        Context();
        Context(bool enableCuda);
        ~Context();
        !Context();

        void EnableCudaAcceleration(bool enable);
        bool IsCudaAccelerationEnabled();

        // Output header for DDS file
        bool OutputHeader(Surface^ surface, int mipmapCount, CompressionOptions^ compressionOptions, OutputOptions^ outputOptions);

        // Compress surface
        bool Compress(Surface^ surface, int face, int mipmap, CompressionOptions^ compressionOptions, OutputOptions^ outputOptions);

        // Estimate compressed size
        int EstimateSize(Surface^ surface, int mipmapCount, CompressionOptions^ compressionOptions);

        property nvtt::Context* Native { nvtt::Context* get() { return m_pContext; } }
    };

    public ref class NvttUtility abstract sealed
    {
    public:
        static unsigned int GetVersion();
        static String^ GetErrorString(Error error);
        static bool IsCudaSupported();
    };

} // namespace NVTT
} // namespace CodeWalker
