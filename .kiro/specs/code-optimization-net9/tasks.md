# Implementation Plan

- [-] 1. Set up test project and infrastructure



  - [x] 1.1 Create CodeWalker.Tests project with xUnit and FsCheck



    - Add new test project to solution
    - Configure FsCheck.Xunit package
    - Set up project references to CodeWalker.Core
    - _Requirements: Testing Strategy_
  - [ ]* 1.2 Write property test for TryGetValue equivalence
    - **Property 1: TryGetValue Equivalence**
    - **Validates: Requirements 1.1**
  - [ ]* 1.3 Write property test for TryAdd equivalence
    - **Property 2: TryAdd Equivalence**
    - **Validates: Requirements 1.3**

- [x] 2. Optimize dictionary access patterns in CodeWalker.WinForms





  - [x] 2.1 Refactor STNodeTreeView.cs ContainsKey patterns


    - Replace `ContainsKey` + indexer with `TryGetValue` in RemoveNode method
    - Replace pattern in indexer setter
    - _Requirements: 1.1_
  - [x] 2.2 Refactor STNodeEditorPanel.cs ContainsKey patterns


    - Replace `ContainsKey` + indexer with `TryGetValue` in SetConnectionStatusText
    - _Requirements: 1.1_

  - [x] 2.3 Refactor STNodeEditor.cs ContainsKey patterns

    - Replace `ContainsKey` + indexer with `TryGetValue` in DisConnectionHover
    - Replace pattern in node loading code
    - Replace pattern in SetTypeColor
    - _Requirements: 1.1_
  - [x] 2.4 Refactor STNode.cs ContainsKey patterns


    - Replace multiple `ContainsKey` + indexer patterns in OnLoadNode with `TryGetValue`
    - _Requirements: 1.1_
  - [ ]* 2.5 Write unit tests for dictionary pattern changes
    - Test STNodeEditor dictionary operations
    - _Requirements: 1.1_

- [x] 3. Checkpoint - Ensure all tests pass





  - Ensure all tests pass, ask the user if questions arise.

- [x] 4. Optimize dictionary access patterns in CodeWalker.Core





  - [x] 4.1 Refactor GameFileCache.cs ContainsKey patterns


    - Replace `ContainsKey` + indexer patterns with `TryGetValue`
    - _Requirements: 1.1_
  - [x] 4.2 Refactor ProjectForm.cs ContainsKey patterns


    - Replace `ContainsKey` + indexer with `TryGetValue` in GetVisibleMloInstance
    - _Requirements: 1.1_
  - [ ]* 4.3 Write property test for collection expression equivalence
    - **Property 4: Collection Expression Equivalence**
    - **Validates: Requirements 3.1, 3.2**

- [x] 5. Modernize string formatting in CodeWalker.Core





  - [x] 5.1 Convert string.Format to interpolation in CacheDatFile.cs


    - Replace all `string.Format` calls with string interpolation
    - _Requirements: 2.1_
  - [x] 5.2 Convert string.Format to interpolation in RelFile.cs


    - Replace `string.Format` calls in ToString methods
    - _Requirements: 2.1_
  - [x] 5.3 Convert string.Format to interpolation in other Core files


    - Update Water.cs, Jenk.cs, WatermapFile.cs, Particle.cs, Rbf.cs, Drawable.cs, Bounds.cs, Nav.cs, Archetype.cs
    - _Requirements: 2.1_
  - [x] 5.4 Convert string.Format to interpolation in DDSIO.cs


    - Replace exception message formatting
    - _Requirements: 2.1_
  - [ ]* 5.5 Write property test for string interpolation equivalence
    - **Property 3: String Interpolation Equivalence**
    - **Validates: Requirements 2.1**

- [x] 6. Checkpoint - Ensure all tests pass





  - Ensure all tests pass, ask the user if questions arise.

- [x] 7. Modernize collection initialization






  - [x] 7.1 Update FbxConverter.cs collection initialization

    - Replace `new List<T>()` with collection expressions where items are added immediately
    - _Requirements: 3.1, 3.2_
  - [x] 7.2 Update Space.cs collection initialization


    - Replace `new List<T>()` with collection expressions
    - _Requirements: 3.1, 3.2_
  - [x] 7.3 Update other Core files collection initialization


    - Update Heightmaps.cs, PopZones.cs collection patterns
    - _Requirements: 3.1, 3.2_
  - [ ]* 7.4 Write unit tests for collection initialization changes
    - Verify collections contain expected elements
    - _Requirements: 3.1, 3.2_

- [x] 8. Upgrade lock mechanisms to System.Threading.Lock





  - [x] 8.1 Upgrade lock objects in WorldForm.cs


    - Change `object` sync roots to `Lock` type
    - Update lock statements to use new Lock semantics
    - _Requirements: 4.1, 4.2_
  - [x] 8.2 Upgrade lock objects in PedsForm.cs


    - Change `object` sync roots to `Lock` type
    - _Requirements: 4.1, 4.2_
  - [x] 8.3 Upgrade lock objects in Renderer.cs


    - Change rendersyncroot to `Lock` type
    - _Requirements: 4.1, 4.2_

  - [x] 8.4 Upgrade lock objects in Space.cs

    - Change lockObj to `Lock` type
    - _Requirements: 4.1, 4.2_
  - [x] 8.5 Upgrade Monitor.TryEnter patterns


    - Replace `Monitor.TryEnter` with `Lock.TryEnter` in WorldForm.cs and PedsForm.cs
    - _Requirements: 4.3_
  - [ ]* 8.6 Write property test for lock mutual exclusion
    - **Property 5: Lock Mutual Exclusion Equivalence**
    - **Validates: Requirements 4.1**
  - [ ]* 8.7 Write property test for Lock.TryEnter equivalence
    - **Property 6: Lock.TryEnter Equivalence**
    - **Validates: Requirements 4.3**
-

- [x] 9. Checkpoint - Ensure all tests pass




  - Ensure all tests pass, ask the user if questions arise.

- [x] 10. Modernize null handling patterns






  - [x] 10.1 Apply null-conditional operators in STNodeEditor files

    - Replace explicit null checks with `?.` operator where appropriate
    - _Requirements: 5.1_
  - [x] 10.2 Apply null-conditional operators in Core files


    - Update Fbx.cs, ReadOnlyPropertyGrid.cs, ProjectPanel.cs
    - _Requirements: 5.1_
  - [x] 10.3 Apply null-coalescing operators


    - Replace `if (x == null) x = y` patterns with `x ??= y`
    - _Requirements: 5.2_

  - [x] 10.4 Apply pattern matching for type checks

    - Replace `if (obj is Type) { var t = (Type)obj; }` with `if (obj is Type t)`
    - _Requirements: 5.3_
  - [ ]* 10.5 Write property test for null-conditional equivalence
    - **Property 7: Null-Conditional Equivalence**
    - **Validates: Requirements 5.1**
  - [ ]* 10.6 Write property test for null-coalescing equivalence
    - **Property 8: Null-Coalescing Equivalence**
    - **Validates: Requirements 5.2**
  - [ ]* 10.7 Write property test for pattern matching equivalence
    - **Property 9: Pattern Matching Equivalence**
    - **Validates: Requirements 5.3**

- [x] 11. Optimize array and memory operations





  - [x] 11.1 Replace loop-based array copying with Span/Array.Copy


    - Update STNodePropertyAttribute.cs array parsing
    - Update STNode.cs array operations
    - _Requirements: 6.1_
  - [x] 11.2 Evaluate ArrayPool usage for large allocations


    - Identify large temporary array allocations in Core
    - Apply ArrayPool where beneficial
    - _Requirements: 6.2_
  - [ ]* 11.3 Write property test for array copy equivalence
    - **Property 10: Array Copy Equivalence**
    - **Validates: Requirements 6.1**
  - [ ]* 11.4 Write property test for Span encoding equivalence
    - **Property 11: Span Encoding Equivalence**
    - **Validates: Requirements 6.3**

- [x] 12. Checkpoint - Ensure all tests pass





  - Ensure all tests pass, ask the user if questions arise.

- [x] 13. Optimize LINQ usage





  - [x] 13.1 Consolidate chained LINQ operations


    - Identify and consolidate multiple Where/Select chains
    - _Requirements: 7.2_
  - [x] 13.2 Evaluate loop alternatives for performance-critical LINQ


    - Review LINQ in GameFileCache.cs hot paths
    - Replace with loops where beneficial
    - _Requirements: 7.3_
  - [ ]* 13.3 Write property test for LINQ consolidation equivalence
    - **Property 12: LINQ Consolidation Equivalence**
    - **Validates: Requirements 7.2**
  - [ ]* 13.4 Write property test for loop vs LINQ equivalence
    - **Property 13: Loop vs LINQ Equivalence**
    - **Validates: Requirements 7.3**

- [-] 14. Improve async/await patterns




  - [x] 14.1 Add ConfigureAwait(false) to library async methods

    - Update DDSIO.cs async methods
    - Update YtdFile.cs async methods
    - _Requirements: 8.2_
  - [-] 14.2 Add exception handling for fire-and-forget tasks

    - Wrap Task.Run calls in ExploreForm.cs with exception handling
    - Wrap Task.Run calls in WorldForm.cs with exception handling
    - _Requirements: 8.3_
  - [ ]* 14.3 Write property test for fire-and-forget exception handling
    - **Property 14: Fire-and-Forget Exception Handling**
    - **Validates: Requirements 8.3**

- [x] 15. Final Checkpoint - Ensure all tests pass




  - Ensure all tests pass, ask the user if questions arise.
