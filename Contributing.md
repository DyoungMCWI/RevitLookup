## Table of contents

<!-- TOC -->
* [Fork, Clone, Branch and Create your PR](#fork-clone-branch-and-create-your-pr)
* [Rules](#rules)
* [Building](#building)
  * [Prerequisites](#prerequisites)
  * [Initialize and update submodules](#initialize-and-update-submodules)
  * [Compiling Source Code](#compiling-source-code)
  * [Creating MSI installer on a local machine](#creating-msi-installer-on-a-local-machine)
* [Publish a new Release](#publish-a-new-release)
  * [Creating a new release from the IDE](#creating-a-new-release-from-the-ide)
  * [Creating a new release from the Terminal](#creating-a-new-release-from-the-terminal)
  * [Creating a new release on GitHub](#creating-a-new-release-on-github)
* [Architecture](#architecture)
  * [Descriptors](#descriptors)
  * [IDescriptorResolver](#idescriptorresolver)
    * [Single Value Resolution](#single-value-resolution)
    * [Multiple Value Resolution](#multiple-value-resolution)
    * [Disabling Methods](#disabling-methods)
    * [Targeting Specific Overloads](#targeting-specific-overloads)
  * [IDescriptorExtension](#idescriptorextension)
  * [IDescriptorRedirector](#idescriptorredirector)
  * [IDescriptorCollector](#idescriptorcollector)
  * [IDescriptorConnector](#idescriptorconnector)
  * [UI Styling](#ui-styling)
<!-- TOC -->

## Fork, Clone, Branch and Create your PR

1. Fork the repo if you haven't already
2. Clone your fork locally
3. Create & push a feature branch
4. Create a [Draft Pull Request (PR)](https://github.blog/2019-02-14-introducing-draft-pull-requests/)
5. Work on your changes

## Rules

- Follow the pattern of what you already see in the code.
- When adding new classes/methods/changing existing code:
    - Run the debugger and make sure everything works.
    - Add appropriate XML documentation comments.
    - Follow C# coding conventions.
- The naming should be descriptive and direct, giving a clear idea of the functionality.
- Keep commits atomic and write meaningful commit messages.
- Follow semantic versioning guidelines for releases.
- Address code review feedback promptly.

## Building

### Prerequisites

- Windows 10 April 2018 Update (version 1803) or newer.
- One of the following IDEs:
    - JetBrains Rider 2023.3 or newer.
    - Visual Studio 2022 (any edition) with following workloads:
        - .NET desktop development.
        - .NET Core cross-platform development.
- Required .NET SDKs:
    - [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
    - [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet)
- Git for version control.

### Initialize and update submodules

After cloning the project, run this command to update all related modules:

```shell
git submodule update --init --force --recursive
cd source/LookupEngine
git sparse-checkout init --cone
git sparse-checkout set source/
cd ../LookupEngine.UI
git sparse-checkout init --cone
git sparse-checkout set source/
```

### Compiling Source Code

We recommend JetBrains Rider as preferred IDE, since it has outstanding .NET support. If you don't have Rider installed, you can download it
from [here](https://www.jetbrains.com/rider/).

1. Open IDE.
2. Open the solution file `LookupEngine.sln`.
3. In the `Solutions Configuration` drop-down menu, select `Debug` configuration.
4. After the solution loads, you can build it by clicking on `Build -> Build Solution`.
5. Use the `Debug` button to start debugging.

### Creating MSI installer on a local machine

To build the RevitLookup for all versions and create the installer, use [NUKE](https://github.com/nuke-build/nuke)

To execute your NUKE build locally, you can follow these steps:

1. **Install NUKE as a global tool**. First, make sure you have NUKE installed as a global tool. You can install it using dotnet CLI:

    ```powershell
    dotnet tool install Nuke.GlobalTool --global
    ```

   You only need to do this once on your machine.

2. **Navigate to the solution directory**. Open a terminal / command prompt and navigate to RevitLookup root directory.
3. **Run the build**. Once you have navigated to your solution directory, you can run the NUKE build by calling:

   Compile:
   ```powershell
   nuke
   ```

   Create an installer:
   ```powershell
   nuke createinstaller
   ```
   
   This command will execute the NUKE build, defined in the RevitLookup project.

## Publish a new Release

Releases are managed by creating new Git tags.
Tags act as unique identifiers for specific versions, with the ability to roll back to earlier versions.

Tags must follow the format `version` or `version-stage.n.date` for pre-releases, where:

- **version** specifies the version of the release:
    - `1.0.0`
    - `2.3.0`
- **stage** specifies the release stage:
    - `alpha` - represents early iterations that may be unstable or incomplete.
    - `beta` - represents a feature-complete version but may still contain some bugs.
- **n** prerelease increment (optional):
    - `1` - first alpha prerelease
    - `2` - second alpha prerelease
- **date** specifies the date of the pre-release (optional):
    - `250101`
    - `20250101`

For example:

| Stage   | Version                |
|---------|------------------------|
| Alpha   | 1.0.0-alpha            |
| Alpha   | 1.0.0-alpha.1.20250101 |
| Beta    | 1.0.0-beta.2.20250101  |
| Release | 1.0.0                  |

### Creating a new release from the IDE

Publishing a release begins with the creation of a new tag:

1. Open JetBrains Rider.
2. Navigate to the **Git** tab.
3. Click **New Tag...** and create a new tag.

   ![image](https://github.com/user-attachments/assets/19c11322-9f95-45e5-8fe6-defa36af59c4)

4. Navigate to the **Git** panel.
5. Expand the **Tags** section.
6. Right-click on the newly created tag and select **Push to origin**.

   ![image](https://github.com/user-attachments/assets/b2349264-dd76-4c21-b596-93110f1f16cb)

   This process will trigger the Release workflow and create a new release on GitHub.

### Creating a new release from the Terminal

Alternatively, you can create and push tags using the terminal:

1. Navigate to the repository root and open the terminal.
2. Use the following command to create a new tag:
   ```shell
   git tag 'version'
   ```

   Replace `version` with the desired version, e.g., `1.0.0`.
3. Push the newly created tag to the remote repository using the following command:
   ```shell
   git push origin 'version'
   ```
### Creating a new release on GitHub

To create releases directly on GitHub:

1. Navigate to the **Actions** section on the repository page.
2. Select **Publish Release** workflow.
3. Click **Run workflow** button.
4. Specify the release version and click **Run**.

   ![image](https://github.com/user-attachments/assets/088388c1-6055-4d21-8d22-70f047d8f104)

> To create a release, changelog for the release version is required.

To update the changelog:

1. Navigate to the solution root.
2. Open the file **Changelog.md**.
3. Add a section for your version. The version separator is the `#` symbol.
4. Specify the release number e.g. `# 1.0.0` or `# Release v1.0.0`, the format does not matter, the main thing is that it contains the version.
5. In the lines below, write a changelog for your version, style to your taste. For example, you will find changelog for version 1.0.0, do the same.

## Architecture

RevitLookup is built on the LookupEngine framework, which provides system for analyzing object structures at runtime. This section explains how you can extend and modify core components of the project.

### Descriptors

Descriptors are specialized classes that define how objects should be handled by the LookupEngine. Each descriptor is responsible for a specific type or family of types in Revit.

To add a descriptor for a new class:

1. Create a new descriptor class in the appropriate folder under `RevitLookup\Core\Decomposition\Descriptors`
2. Register the descriptor in the descriptor map located at `RevitLookup\Core\Decomposition\DescriptorMap.cs`

### IDescriptorResolver

This interface allows descriptors to control how methods and properties with parameters are evaluated. In RevitLookup, `Document` serves as the context for resolution.

#### Single Value Resolution

```csharp
// RevitLookup\Core\Decomposition\Descriptors\ElementDescriptor.cs
public class ElementDescriptor(Element element) : Descriptor, IDescriptorResolver<Document>
{
    public virtual Func<Document, IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Element.IsHidden) => ResolveIsHidden,
            _ => null
        };

        IVariant ResolveIsHidden(Document context)
        {
            return Variants.Value(element.IsHidden(context.ActiveView), "Active view");
        }
    }
}
```

#### Multiple Value Resolution

```csharp
// RevitLookup\Core\Decomposition\Descriptors\ElementDescriptor.cs
public class ElementDescriptor(Element element) : Descriptor, IDescriptorResolver<Document>
{
    public virtual Func<Document, IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Element.GetBoundingBox) => ResolveBoundingBox,
            _ => null
        };

        IVariant ResolveBoundingBox(Document context)
        {
            return Variants.Values<BoundingBoxXYZ>(2)
                .Add(element.get_BoundingBox(null), "Model")
                .Add(element.get_BoundingBox(context.ActiveView), "Active view")
                .Consume();
        }
    }
}
```

#### Disabling Methods

```csharp
// RevitLookup\Core\Decomposition\Descriptors\DocumentDescriptor.cs
public class DocumentDescriptor(Document document) : Descriptor, IDescriptorResolver
{
    public virtual Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.Close) => Variants.Disabled,
            _ => null
        };
    }
}
```

#### Targeting Specific Overloads

```csharp
// RevitLookup\Core\Decomposition\Descriptors\EntityDescriptor.cs
public sealed class EntityDescriptor(Entity entity) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Entity.Get) when parameters.Length == 1 &&
                                    parameters[0].ParameterType == typeof(string) => ResolveGetByField,
            _ => null
        };
        
        IVariant ResolveGetByField()
        {
            return Variants.Value(entity.Get("Parameter Name"));
        }
    }
}
```

### IDescriptorExtension

This interface allows adding custom methods and properties to objects that don't exist in the original type. In RevitLookup, this can use the `Document` context to provide additional functionality.

```csharp
// RevitLookup\Core\Decomposition\Descriptors\ColorDescriptor.cs
public sealed class ColorDescriptor(Color color) : Descriptor, IDescriptorExtension
{
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("HEX", () => Variants.Value(ColorRepresentationUtils.ColorToHex(color)));
        manager.Register("RGB", () => Variants.Value(ColorRepresentationUtils.ColorToRgb(color)));
        manager.Register("HSL", () => Variants.Value(ColorRepresentationUtils.ColorToHsl(color)));
    }
}
```

With context:

```csharp
// RevitLookup\Core\Decomposition\Descriptors\SchemaDescriptor.cs
public sealed class SchemaDescriptor(Schema schema) : Descriptor, IDescriptorExtension<Document>
{
    public void RegisterExtensions(IExtensionManager<Document> manager)
    {
        manager.Register("GetElements", context => Variants.Value(context
            .GetElements()
            .WherePasses(new ExtensibleStorageFilter(schema.GUID))
            .ToElements()));
    }
}
```

### IDescriptorRedirector

This interface lets a descriptor redirect to another object. For example, converting from an ID to the actual element.

```csharp
// RevitLookup\Core\Decomposition\Descriptors\ElementIdDescriptor.cs
public sealed class ElementIdDescriptor(ElementId elementId) : Descriptor, IDescriptorRedirector<Document>
{
    public bool TryRedirect(string target, Document context, out object result)
    {
        if (elementId == ElementId.InvalidElementId) 
        {
            result = null;
            return false;
        }

        result = elementId.ToElement(context);
        return result != null;
    }
}
```

### IDescriptorCollector

This interface serves as a marker indicating that the descriptor can decompose the object's members. It's essential for allowing users to inspect an object's internal structure.

```csharp
// RevitLookup\Core\Decomposition\Descriptors\ApplicationDescriptor.cs
public sealed class ApplicationDescriptor : Descriptor, IDescriptorCollector
{
    public ApplicationDescriptor(Application application)
    {
        Name = application.VersionName;
    }
}
```

### IDescriptorConnector

This interface enables integration with the RevitLookup UI, allowing descriptors to add custom context menu options and commands.

```csharp
// RevitLookup\Core\Decomposition\Descriptors\ElementDescriptor.cs
public sealed class ElementDescriptor : Descriptor, IDescriptorConnector
{
    public void RegisterMenu(ContextMenu contextMenu)
    {
        contextMenu.AddMenuItem()
            .SetHeader("Show element")
            .SetAvailability(_element is not ElementType)
            .SetCommand(_element, element =>
            {
                Context.UiDocument.ShowElements(element);
                Context.UiDocument.Selection.SetElementIds([element.Id]);
            })
            .AddShortcut(ModifierKeys.Alt, Key.F7);
    }
}
```

### UI Styling

The application UI is data-template based, with templates customizable for different data types. Templates are located in `RevitLookup\Styles\ComponentStyles` directory.

To customize the display of a specific type:

1. Create a DataTemplate in a XAML file within the Controls directory

```xml
// RevitLookup\Styles\ComponentStyles\ObjectsTree\TreeGroupTemplates.xaml
<DataTemplate
    x:Key="DefaultSummaryTreeItemTemplate"
    DataType="{x:Type decomposition:ObservableDecomposedObject}">
    <ui:TextBlock
        FontTypography="Caption"
        Text="{Binding .,
            Converter={valueConverters:SingleDescriptorLabelConverter},
            Mode=OneTime}" />
</DataTemplate>
```

2. Add a selector rule in the `TemplateSelector` class:

```csharp
// RevitLookup\Styles\ComponentStyles\ObjectsTree\TreeViewItemTemplateSelector.cs
public sealed class TreeViewItemTemplateSelector : DataTemplateSelector
{
    /// <summary>
    ///     Tree view row style selector
    /// </summary>
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is null) return null;

        var presenter = (FrameworkElement) container;
        var decomposedObject = (ObservableDecomposedObject) item;
        var templateName = decomposedObject.RawValue switch
        {
            Color => "SummaryMediaColorItemTemplate",
            _ => "DefaultSummaryTreeItemTemplate"
        };

        return (DataTemplate) presenter.FindResource(templateName);
    }
}
```

For custom visualization of specific data types, create specialized templates following the pattern above and register them in the appropriate style selectors.