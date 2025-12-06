## Tech Stack

- **Language**: C# with .NET 9.0
- **UI Framework**: Windows Forms (WinForms) with WPF support
- **Graphics**: DirectX 11 via SharpDX (Direct3D11, Direct2D1, XAudio2, XInput)
- **Shaders**: HLSL (compiled to .cso files)
- **Build System**: MSBuild via Visual Studio solution (.sln)

## Key Dependencies

- SharpDX 4.2.0 (DirectX wrapper)
- DockPanelSuite.ThemeVS2015 3.1.1
- FCTB 2.16.24 (Fast Colored TextBox)
- WinForms.DataVisualization 1.10.0

## Project Structure

The solution contains multiple projects:
- **CodeWalker.Core**: Core game file parsing and data structures (library)
- **CodeWalker**: Main application with UI and rendering
- **CodeWalker.WinForms**: Shared WinForms controls and utilities
- **CodeWalker.Shaders**: HLSL shader compilation (C++ vcxproj)
- **CodeWalker.Peds**: Standalone peds viewer
- **CodeWalker.Vehicles**: Standalone vehicles viewer
- **CodeWalker.RPFExplorer**: Standalone RPF explorer
- **CodeWalker.ModManager**: Mod management utility
- **CodeWalker.Gen9Converter**: Gen9 asset converter
- **CodeWalker.ErrorReport**: Error reporting utility

## Common Commands

Build the solution:
```
msbuild CodeWalker.sln /p:Configuration=Release /p:Platform=x64
```

Build specific project:
```
msbuild CodeWalker/CodeWalker.csproj /p:Configuration=Release
```

Restore NuGet packages:
```
dotnet restore CodeWalker.sln
```

Build shaders (requires Visual Studio C++ tools):
```
msbuild CodeWalker.Shaders/CodeWalker.Shaders.vcxproj /p:Configuration=Release /p:Platform=x64
```

## Language Features

- C# latest language version
- Nullable reference types enabled
- Global using directives (see GlobalUsings.cs files)
