# .github/copilot-instructions.md

## Mod Overview and Purpose

**Mod Name:** New Zone Tools (Continued)

**Description:**  
The "New Zone Tools (Continued)" mod for RimWorld enhances the gameplay by introducing new tools that streamline zone management and construction tasks. Originally developed by Britnoth and now maintained as a continuation, this mod aims to improve the efficiency of managing zones and selected tasks in RimWorld colonies.

## Key Features and Systems

1. **Uniform Grow Tool:**  
   - Allows players to select a grow zone that matches the fertility of an initially selected tile.
   - This feature is also available as part of the TD Enhancement Pack, providing uniformity and consistency in agriculture management.

2. **Chequer Cancel Tool:**  
   - Cancels construction or blueprints in a chessboard pattern.
   - Optimizes construction management by providing more control over canceling tasks, especially in complex building plans.

3. **Replace Wall Tool:**  
   - Enables players to place new wall blueprints over existing walls, streamlining wall upgrades and modifications.
   - Note: The use of the Replace Stuff mod is recommended for enhanced functionality in wall replacement.

4. **Cut Blighted Tool:**  
   - Selects only plants with blight to be marked for cutting, aiding in effective disease management.
   - It is advised to use the Allow Tool mod for similar functionality and broader utility.

## Coding Patterns and Conventions

- **C# File Structure:**  
  The project is structured into C# files, each handling different tool functionalities such as `Designator_ZoneAdd_GrowingSame` for the Uniform Grow Tool and `Designator_CancelChequer` for the Chequer Cancel Tool.
  
- **Naming Conventions:**  
  Classes and methods follow CamelCase naming conventions for clarity and consistency (e.g., `CanDesignateCell`, `Designator_CancelChequer`).

- **Member Usage:**  
  Methods are organized according to their functionality, such as designation checks (`CanDesignateCell`) and updates during selection changes (`SelectedUpdate`).

## XML Integration

- **XML File:**  
  The mod includes an XML file `DesignationCategories.xml` for defining and categorizing designations. It plays a crucial role in integrating the tools into the game's user interface, ensuring seamless interaction within the game.

## Harmony Patching

- **Purpose:**  
  Harmony is used to patch existing game methods, allowing the mod to inject new functionality or alter existing behavior without modifying the core game files.

- **Approach:**  
  The mod applies Harmony patches to intercept and enhance specific game functions related to designations and zoning, ensuring compatibility and extended features.

## Suggestions for Copilot

1. **Assist with Code Suggestions:**  
   Copilot can provide inline code suggestions for creating additional features or optimizing existing C# methods.

2. **Enhance XML Customization:**  
   Copilot can assist in writing and organizing new XML definitions to further extend the mod's capabilities.

3. **Debugging Assistance:**  
   Copilot can help quickly identify and rectify potential issues in both C# and XML, improving mod stability and performance.

4. **Implement Additional Features:**  
   Suggest new tools or features based on existing patterns or extend current functionality, facilitating continuous improvement of the mod.

5. **Improve Documentation:**  
   Assist in creating detailed inline documentation and comments within the codebase to enhance readability and maintainability.

By following these instructions and leveraging GitHub Copilot, developers can maintain and enhance the "New Zone Tools (Continued)" mod effectively, ensuring it remains a valuable asset to the RimWorld modding community.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

