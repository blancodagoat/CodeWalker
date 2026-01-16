#include "NvttWrapper.h"

// Include NVTT 3 headers
#include <nvtt/nvtt.h>

#include <vcclr.h>
#include <string.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace CodeWalker {
namespace NVTT {

    // Native output handler that forwards to managed handler
    class ManagedOutputHandler : public nvtt::OutputHandler
    {
    private:
        gcroot<IOutputHandler^> m_handler;
        gcroot<array<Byte>^> m_buffer;

    public:
        ManagedOutputHandler(IOutputHandler^ handler) : m_handler(handler), m_buffer(nullptr) {}

        virtual void beginImage(int size, int width, int height, int depth, int face, int miplevel) override
        {
            m_handler->BeginImage(size, width, height, depth, face, miplevel);
        }

        virtual bool writeData(const void* data, int size) override
        {
            array<Byte>^ buffer = m_buffer;
            if (buffer == nullptr || buffer->Length < size)
            {
                buffer = gcnew array<Byte>(size);
                m_buffer = buffer;
            }
            Marshal::Copy(IntPtr(const_cast<void*>(data)), buffer, 0, size);
            return m_handler->WriteData(buffer, 0, size);
        }

        virtual void endImage() override
        {
            m_handler->EndImage();
        }
    };

    // Native output handler for unsafe (pointer-based) managed handler
    class ManagedUnsafeOutputHandler : public nvtt::OutputHandler
    {
    private:
        gcroot<IUnsafeOutputHandler^> m_handler;

    public:
        ManagedUnsafeOutputHandler(IUnsafeOutputHandler^ handler) : m_handler(handler) {}

        virtual void beginImage(int size, int width, int height, int depth, int face, int miplevel) override
        {
            m_handler->BeginImage(size, width, height, depth, face, miplevel);
        }

        virtual bool writeData(const void* data, int size) override
        {
            return m_handler->WriteData(IntPtr(const_cast<void*>(data)), size);
        }

        virtual void endImage() override
        {
            m_handler->EndImage();
        }
    };

    // Native error handler that forwards to managed handler
    class ManagedErrorHandler : public nvtt::ErrorHandler
    {
    private:
        gcroot<IErrorHandler^> m_handler;

    public:
        ManagedErrorHandler(IErrorHandler^ handler) : m_handler(handler) {}

        virtual void error(nvtt::Error e) override
        {
            m_handler->HandleError(static_cast<Error>(e));
        }
    };

    CompressionOptions::CompressionOptions()
    {
        m_pOptions = new nvtt::CompressionOptions();
        m_disposed = false;
    }

    CompressionOptions::~CompressionOptions()
    {
        this->!CompressionOptions();
    }

    CompressionOptions::!CompressionOptions()
    {
        if (!m_disposed && m_pOptions != nullptr)
        {
            delete m_pOptions;
            m_pOptions = nullptr;
            m_disposed = true;
        }
    }

    void CompressionOptions::Reset()
    {
        m_pOptions->reset();
    }

    void CompressionOptions::SetFormat(Format format)
    {
        m_pOptions->setFormat(static_cast<nvtt::Format>(format));
    }

    void CompressionOptions::SetQuality(Quality quality)
    {
        m_pOptions->setQuality(static_cast<nvtt::Quality>(quality));
    }

    void CompressionOptions::SetColorWeights(float red, float green, float blue)
    {
        m_pOptions->setColorWeights(red, green, blue);
    }

    void CompressionOptions::SetColorWeights(float red, float green, float blue, float alpha)
    {
        m_pOptions->setColorWeights(red, green, blue, alpha);
    }

    void CompressionOptions::SetPixelFormat(unsigned int bitCount, unsigned int rmask, unsigned int gmask, unsigned int bmask, unsigned int amask)
    {
        m_pOptions->setPixelFormat(bitCount, rmask, gmask, bmask, amask);
    }

    void CompressionOptions::SetPixelFormat(unsigned char rsize, unsigned char gsize, unsigned char bsize, unsigned char asize)
    {
        m_pOptions->setPixelFormat(rsize, gsize, bsize, asize);
    }

    void CompressionOptions::SetQuantization(bool colorDithering, bool alphaDithering, bool binaryAlpha)
    {
        m_pOptions->setQuantization(colorDithering, alphaDithering, binaryAlpha);
    }

    void CompressionOptions::SetQuantization(bool colorDithering, bool alphaDithering, bool binaryAlpha, int alphaThreshold)
    {
        m_pOptions->setQuantization(colorDithering, alphaDithering, binaryAlpha, alphaThreshold);
    }

    unsigned int CompressionOptions::GetD3D9Format()
    {
        return m_pOptions->d3d9Format();
    }

    OutputOptions::OutputOptions()
    {
        m_pOptions = new nvtt::OutputOptions();
        m_pOutputHandler = nullptr;
        m_pErrorHandler = nullptr;
        m_disposed = false;
    }

    OutputOptions::~OutputOptions()
    {
        this->!OutputOptions();
    }

    OutputOptions::!OutputOptions()
    {
        if (!m_disposed)
        {
            if (m_pOptions != nullptr)
            {
                delete m_pOptions;
                m_pOptions = nullptr;
            }
            if (m_pOutputHandler != nullptr)
            {
                delete m_pOutputHandler;
                m_pOutputHandler = nullptr;
            }
            if (m_pErrorHandler != nullptr)
            {
                delete m_pErrorHandler;
                m_pErrorHandler = nullptr;
            }
            if (m_outputHandlerHandle.IsAllocated)
            {
                m_outputHandlerHandle.Free();
            }
            if (m_errorHandlerHandle.IsAllocated)
            {
                m_errorHandlerHandle.Free();
            }
            m_disposed = true;
        }
    }

    void OutputOptions::Reset()
    {
        m_pOptions->reset();
    }

    void OutputOptions::SetFileName(String^ fileName)
    {
        if (fileName == nullptr)
        {
            m_pOptions->setFileName(nullptr);
        }
        else
        {
            IntPtr pFileName = Marshal::StringToHGlobalAnsi(fileName);
            try
            {
                m_pOptions->setFileName(static_cast<const char*>(pFileName.ToPointer()));
            }
            finally
            {
                Marshal::FreeHGlobal(pFileName);
            }
        }
    }

    void OutputOptions::SetOutputHandler(IOutputHandler^ handler)
    {
        // Clean up existing handler
        if (m_pOutputHandler != nullptr)
        {
            delete m_pOutputHandler;
            m_pOutputHandler = nullptr;
        }
        if (m_outputHandlerHandle.IsAllocated)
        {
            m_outputHandlerHandle.Free();
        }

        if (handler != nullptr)
        {
            m_outputHandlerHandle = GCHandle::Alloc(handler);
            m_pOutputHandler = new ManagedOutputHandler(handler);
            m_pOptions->setOutputHandler(m_pOutputHandler);
        }
        else
        {
            m_pOptions->setOutputHandler(nullptr);
        }
    }

    void OutputOptions::SetOutputHandler(IUnsafeOutputHandler^ handler)
    {
        // Clean up existing handler
        if (m_pOutputHandler != nullptr)
        {
            delete m_pOutputHandler;
            m_pOutputHandler = nullptr;
        }
        if (m_outputHandlerHandle.IsAllocated)
        {
            m_outputHandlerHandle.Free();
        }

        if (handler != nullptr)
        {
            m_outputHandlerHandle = GCHandle::Alloc(handler);
            m_pOutputHandler = new ManagedUnsafeOutputHandler(handler);
            m_pOptions->setOutputHandler(m_pOutputHandler);
        }
        else
        {
            m_pOptions->setOutputHandler(nullptr);
        }
    }

    void OutputOptions::SetErrorHandler(IErrorHandler^ handler)
    {
        // Clean up existing handler
        if (m_pErrorHandler != nullptr)
        {
            delete m_pErrorHandler;
            m_pErrorHandler = nullptr;
        }
        if (m_errorHandlerHandle.IsAllocated)
        {
            m_errorHandlerHandle.Free();
        }

        if (handler != nullptr)
        {
            m_errorHandlerHandle = GCHandle::Alloc(handler);
            m_pErrorHandler = new ManagedErrorHandler(handler);
            m_pOptions->setErrorHandler(m_pErrorHandler);
        }
        else
        {
            m_pOptions->setErrorHandler(nullptr);
        }
    }

    void OutputOptions::SetOutputHeader(bool outputHeader)
    {
        m_pOptions->setOutputHeader(outputHeader);
    }

    void OutputOptions::SetContainer(Container container)
    {
        m_pOptions->setContainer(static_cast<nvtt::Container>(container));
    }

    void OutputOptions::SetSrgbFlag(bool srgb)
    {
        m_pOptions->setSrgbFlag(srgb);
    }

    Surface::Surface()
    {
        m_pSurface = new nvtt::Surface();
        m_disposed = false;
    }

    Surface::~Surface()
    {
        this->!Surface();
    }

    Surface::!Surface()
    {
        if (!m_disposed && m_pSurface != nullptr)
        {
            delete m_pSurface;
            m_pSurface = nullptr;
            m_disposed = true;
        }
    }

    bool Surface::Load(String^ fileName)
    {
        IntPtr pFileName = Marshal::StringToHGlobalAnsi(fileName);
        try
        {
            return m_pSurface->load(static_cast<const char*>(pFileName.ToPointer()));
        }
        finally
        {
            Marshal::FreeHGlobal(pFileName);
        }
    }

    bool Surface::Load(String^ fileName, [Out] bool% hasAlpha)
    {
        IntPtr pFileName = Marshal::StringToHGlobalAnsi(fileName);
        try
        {
            bool alpha = false;
            bool result = m_pSurface->load(static_cast<const char*>(pFileName.ToPointer()), &alpha);
            hasAlpha = alpha;
            return result;
        }
        finally
        {
            Marshal::FreeHGlobal(pFileName);
        }
    }

    bool Surface::LoadFromMemory(array<Byte>^ data)
    {
        pin_ptr<Byte> pinnedData = &data[0];
        return m_pSurface->loadFromMemory(pinnedData, data->Length);
    }

    bool Surface::LoadFromMemory(array<Byte>^ data, [Out] bool% hasAlpha)
    {
        pin_ptr<Byte> pinnedData = &data[0];
        bool alpha = false;
        bool result = m_pSurface->loadFromMemory(pinnedData, data->Length, &alpha);
        hasAlpha = alpha;
        return result;
    }

    bool Surface::SetImage(int width, int height, int depth)
    {
        return m_pSurface->setImage(width, height, depth);
    }

    bool Surface::SetImage(InputFormat format, int width, int height, int depth, IntPtr data)
    {
        return m_pSurface->setImage(static_cast<nvtt::InputFormat>(format), width, height, depth, data.ToPointer());
    }

    bool Surface::SetImage(InputFormat format, int width, int height, int depth, array<Byte>^ data)
    {
        pin_ptr<Byte> pinnedData = &data[0];
        return m_pSurface->setImage(static_cast<nvtt::InputFormat>(format), width, height, depth, pinnedData);
    }

    bool Surface::IsNull::get()
    {
        return m_pSurface->isNull();
    }

    int Surface::Width::get()
    {
        return m_pSurface->width();
    }

    int Surface::Height::get()
    {
        return m_pSurface->height();
    }

    int Surface::Depth::get()
    {
        return m_pSurface->depth();
    }

    TextureType Surface::Type::get()
    {
        return static_cast<TextureType>(m_pSurface->type());
    }

    int Surface::MipmapCount::get()
    {
        return m_pSurface->countMipmaps();
    }

    void Surface::SetWrapMode(WrapMode mode)
    {
        m_pSurface->setWrapMode(static_cast<nvtt::WrapMode>(mode));
    }

    void Surface::SetAlphaMode(AlphaMode mode)
    {
        m_pSurface->setAlphaMode(static_cast<nvtt::AlphaMode>(mode));
    }

    void Surface::SetNormalMap(bool isNormalMap)
    {
        m_pSurface->setNormalMap(isNormalMap);
    }

    bool Surface::BuildNextMipmap(MipmapFilter filter)
    {
        return m_pSurface->buildNextMipmap(static_cast<nvtt::MipmapFilter>(filter));
    }

    bool Surface::BuildNextMipmap(MipmapFilter filter, int minSize)
    {
        return m_pSurface->buildNextMipmap(static_cast<nvtt::MipmapFilter>(filter), minSize);
    }

    bool Surface::CanMakeNextMipmap()
    {
        return m_pSurface->canMakeNextMipmap();
    }

    bool Surface::CanMakeNextMipmap(int minSize)
    {
        return m_pSurface->canMakeNextMipmap(minSize);
    }

    void Surface::Resize(int width, int height, int depth, MipmapFilter filter)
    {
        m_pSurface->resize(width, height, depth, static_cast<nvtt::ResizeFilter>(filter));
    }

    void Surface::ResizeToMaxExtent(int maxExtent, RoundMode mode, MipmapFilter filter)
    {
        m_pSurface->resize(maxExtent, static_cast<nvtt::RoundMode>(mode), static_cast<nvtt::ResizeFilter>(filter));
    }

    void Surface::ToLinear(float gamma)
    {
        m_pSurface->toLinear(gamma);
    }

    void Surface::ToGamma(float gamma)
    {
        m_pSurface->toGamma(gamma);
    }

    void Surface::ToSrgb()
    {
        m_pSurface->toSrgb();
    }

    void Surface::ToLinearFromSrgb()
    {
        m_pSurface->toLinearFromSrgb();
    }

    Surface^ Surface::Clone()
    {
        Surface^ clone = gcnew Surface();
        *clone->m_pSurface = m_pSurface->clone();
        return clone;
    }

    MemoryOutputHandler::MemoryOutputHandler()
    {
        m_stream = gcnew MemoryStream();
        m_currentImageSize = 0;
    }

    void MemoryOutputHandler::BeginImage(int size, int width, int height, int depth, int face, int miplevel)
    {
        m_currentImageSize = size;
    }

    bool MemoryOutputHandler::WriteData(IntPtr data, int size)
    {
        array<Byte>^ buffer = gcnew array<Byte>(size);
        Marshal::Copy(data, buffer, 0, size);
        m_stream->Write(buffer, 0, size);
        return true;
    }

    void MemoryOutputHandler::EndImage()
    {
        // Nothing to do
    }

    array<Byte>^ MemoryOutputHandler::GetData()
    {
        return m_stream->ToArray();
    }

    void MemoryOutputHandler::Reset()
    {
        m_stream->SetLength(0);
        m_stream->Position = 0;
    }

    Context::Context()
    {
        m_pContext = new nvtt::Context(true); // Enable CUDA by default
        m_disposed = false;
    }

    Context::Context(bool enableCuda)
    {
        m_pContext = new nvtt::Context(enableCuda);
        m_disposed = false;
    }

    Context::~Context()
    {
        this->!Context();
    }

    Context::!Context()
    {
        if (!m_disposed && m_pContext != nullptr)
        {
            delete m_pContext;
            m_pContext = nullptr;
            m_disposed = true;
        }
    }

    void Context::EnableCudaAcceleration(bool enable)
    {
        m_pContext->enableCudaAcceleration(enable);
    }

    bool Context::IsCudaAccelerationEnabled()
    {
        return m_pContext->isCudaAccelerationEnabled();
    }

    bool Context::OutputHeader(Surface^ surface, int mipmapCount, CompressionOptions^ compressionOptions, OutputOptions^ outputOptions)
    {
        return m_pContext->outputHeader(*surface->Native, mipmapCount, *compressionOptions->Native, *outputOptions->Native);
    }

    bool Context::Compress(Surface^ surface, int face, int mipmap, CompressionOptions^ compressionOptions, OutputOptions^ outputOptions)
    {
        return m_pContext->compress(*surface->Native, face, mipmap, *compressionOptions->Native, *outputOptions->Native);
    }

    int Context::EstimateSize(Surface^ surface, int mipmapCount, CompressionOptions^ compressionOptions)
    {
        return m_pContext->estimateSize(*surface->Native, mipmapCount, *compressionOptions->Native);
    }

    unsigned int NvttUtility::GetVersion()
    {
        return nvtt::version();
    }

    String^ NvttUtility::GetErrorString(Error error)
    {
        const char* str = nvtt::errorString(static_cast<nvtt::Error>(error));
        return gcnew String(str);
    }

    bool NvttUtility::IsCudaSupported()
    {
        return nvtt::isCudaSupported();
    }

} // namespace NVTT
} // namespace CodeWalker
