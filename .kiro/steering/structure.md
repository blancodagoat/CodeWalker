## Project Organization

CodeWalker follows a multi-project solution structure with clear separation of concerns:

### Core Projects

**CodeWalker.Core/**
- Core library with no UI dependencies
- `GameFiles/`: RPF archive parsing, game file formats (YTD, YDR, YDD, YFT, YMAP, YTYP, YBN, YND, YMT, etc.)
- `Utils/`: Utility classes and helpers
- `World/`: World data structures and management
- `Resources/`: Embedded resources (magic.dat)
- Target: .NET 9.0 library

**CodeWalker/**
- Main application with UI and rendering
- `Forms/`: File-specific viewer forms (AwcForm, YtdForm, ModelForm, etc.)
- `GameFiles/`: UI-specific game file handling
- `Rendering/`: DirectX rendering engine, shaders, vertex types
- `Project/`: Project file management and editing panels
- `Tools/`: Utility tools (audio explorer, texture extraction, Jenkins hash tools, etc.)
- `Utils/`: UI utilities (color picker, input handling, audio synthesis)
- `World/`: World viewer forms and widgets
- `Toolbar/`: Toolbar resources
- Target: .NET 9.0 Windows executable

**CodeWalker.WinForms/**
- Shared WinForms controls and utilities
- Custom controls: PropertyGridFix, TextBoxFix, TreeViewFix, MenuStripFix, etc.
- `STNodeEditor/`: Node editor component
- Target: .NET 9.0 library

**CodeWalker.Shaders/**
- HLSL shader source files (.hlsl, .hlsli)
- Compiled to .cso files in Shaders/ output directory
- Vertex shaders (VS), pixel shaders (PS), compute shaders (CS)
- Target: C++ Visual Studio project

### Standalone Applications

**CodeWalker.Peds/**, **CodeWalker.Vehicles/**, **CodeWalker.RPFExplorer/**
- Lightweight launchers that start CodeWalker in specific modes
- Minimal code, mostly Program.cs entry points

**CodeWalker.ModManager/**
- Standalone mod management application
- Independent from main CodeWalker functionality

**CodeWalker.Gen9Converter/**
- Command-line tool for converting Gen9 assets

**CodeWalker.ErrorReport/**
- Error reporting utility

### Output Structure

**Shaders/**
- Compiled shader binaries (.cso files)
- Loaded at runtime by the rendering engine

**icons/**
- Application icons and markers

### File Naming Conventions

- Forms: `{Purpose}Form.cs` (e.g., ModelForm.cs, YtdForm.cs)
- Game file parsers: `{Format}.cs` or `{Format}File.cs`
- Designer files: `{ClassName}.Designer.cs`
- Resource files: `{ClassName}.resx`

### Configuration Files

- `.csproj`: Project files using SDK-style format
- `GlobalUsings.cs`: Global using directives per project
- `App.config`: Application configuration (legacy)
- `.editorconfig`: Editor configuration
