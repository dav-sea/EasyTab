<p align="center">
  <a href="https://github.com/dav-sea/EasyTab/" target="_blank">
    <img alt="EasyTab" src="https://media.githubusercontent.com/media/dav-sea/EasyTab/master/Media/banner.png" width="1000">
  </a>
</p>

A simple package that allows you to easily implement <kbd>Tab</kbd> key navigation functionality between Selectables. This package is very easy to install and also easy to customize if needed


![Badge](https://img.shields.io/badge/Unity%202019+-black?logo=unity)
[![Badge](https://img.shields.io/badge/Just%20install%20to%20use%20-%20orange)](#installation-methods)
[![Badge](https://img.shields.io/badge/Asset%20Store-black?logo=unity)](https://assetstore.unity.com/packages/tools/gui/easytab-286071)
[![openupm](https://img.shields.io/npm/v/com.dav-sea.easy-tab?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.dav-sea.easy-tab/)
![Badge](https://img.shields.io/badge/TMP-supported-green)   
![Badge](https://img.shields.io/badge/InputSystem%20V1%20(Old)-supported-green)
![Badge](https://img.shields.io/badge/InputSystem%20V2%20(New)-supported-green)
[![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/dav-sea/EasyTab/total?label=downloads&logo=github)](https://github.com/dav-sea/EasyTab/releases/latest)

---

# Table of Contents
- [How to start](#how-to-start)
  - [Installation methods](#installation-methods)
- [Features](#features)
- [Configuration](#configuration)
- [Integration with your tools](#integration-with-your-tools)

# How to start
By default, the package is automatically embedded in PlayerLoop Unity. It is enough to install the package in any convenient way and <kbd>Tab</kbd> key navigation should work

## Installation methods
<details>
<summary> <b style="font-size: large">Method 1.</b> Downlaod and import unitypackage </summary>

| 👉 | Just download `EasyTab.unitypackage` on the [release page](https://github.com/dav-sea/EasyTab/releases) and import it into your project |
|----|-----------------------------------------------------------------------------------------------------------------------------------------|

</details>

<details>
<summary> <b style="font-size: large">Method 2.</b> Via Git URL</summary>

> [!WARNING]
>
> This installation method requires support for query parameters in the url to specify the path to the package.
>
> Requires a unity version of at least **2019.3.4** (If you are using a later version of unity, just use a different installation method)

1. Go to menu ` Window → PackageManager`
2. Click `+` button
3. Paste url 
    ```
    https://github.com/dav-sea/EasyTab.git?path=Assets/Package
    ```
4. Click `Add` button

![image](https://media.githubusercontent.com/media/dav-sea/EasyTab/master/Media/installation-via-git-url.png)

> [!TIP]
>
> You can also specify the target version of the package by adding `#1.0.0` at the end of the URL, for example: `https://github.com/dav-sea/EasyTab.git?path=Assets/Package#1.0.0`

</details>

<details>
<summary> <b style="font-size: large">Method 3.</b> OpenUPM</summary>

You can install [package](https://openupm.com/packages/com.dav-sea.easy-tab/) via OpenUPM client:

```shell
openupm add com.dav-sea.easy-tab
```

</details>

<details>
<summary> <b style="font-size: large">Method 4.</b> AssetStore</summary>

| 👉 | Download asset from [AssetStore page](https://assetstore.unity.com/packages/tools/gui/easytab-286071) |
|----|-------------------------------------------------------------------------------------------------------|

</details>

---

Everything is ready! If the installation of the package was successful, then now you can go to Play Mode and check how the <kbd>Tab</kbd> key navigation works in your project

![gif](https://media.githubusercontent.com/media/dav-sea/EasyTab/master/Media/demo.gif)

# Features
### Just use

✅ Support for all components based on `Selectable`

🚀 Performing useful functionality immediately after installation

🔄 Automatic detection of the navigation sequence by traversing the `Transform` tree

➡️ Support for sequential navigation of the `Selectable` when pressing <kbd>Tab</kbd> key,

🔙 As well as navigation in reverse order when using the <kbd>Shift</kbd>+<kbd>Tab</kbd> key combination

↩️ Processing of the <kbd>Return</kbd> key presses when the focus is on the text field (or custom component)

🔤 Additional support for input fields, including `TextMeshPro` (multiline case and <kbd>Tab</kbd> non-processing when typing)

☰ Dropdown support. Items inside `Dropdown` are automatically navigated cyclically (Roll)

⏮️ Navigate to the last selected object when the focus is not set

💡 Automatic checking of objects for accessibility (alive, active, intractable, canvas-group)

🎨 Canvas support of all types (world-space, screen space overlay, screen space camera)

💫 Automatic navigation between multiple canvases and multiple scenes

⌨️ Support for all input modes (InputSystem V1 (old), InputSystem V2 (new), Both mode)

---
### With `EasyTab` component 

🧩 Easy navigation configuration using the `EasyTab` component for `GameObject` via inspector window

🔀 The ability to explicitly set the navigation sequence using **jumps**

✂️ The ability to exclude `Selectable` or children for navigation

🔠 The ability to override navigation options

🚧 The ability to determine the behavior when reaching the boundaries (🔁 Roll / 🔛 Clamp / ⤴️ Escape)

---
### Advanced </>

🔌 The package code is defined in the `EasyTab.Runtime` assembly (don't forget to [include](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html#reference-another-assembly) it in your assembly)

🔧 The ability to change all fields of the `EasyTab` component through the code

⚙️ The ability to customize global behavior using `EasyTabIntegration.Globally`

🕹️ The ability to independently control navigation through the code

✨ The ability to define additional conditions for the availability of objects (by defining your own `TransformDriver`)

# Configuration
The package provides several configuration levels: `EasyTab` (component), `EasyTabIntegration`, `EasyTabSolver`

### Configuration via `EasyTab` component
Component configuration is a universal way to easily and quickly configure navigation, because for this you just need to add the component to the `GameObject` and set the necessary parameters using the inspector or code.

It is great in cases where you need to define behavior in a local area of your project (for example, on a specific prefab or window)

---
#### SelectableRecognition
You can ignore `Selectable` on the same `GameObject`.

#### BorderMode
You can determine what the solver should do in cases when the extreme children (first or last) are reached when traversing the `Transform` tree

![image](https://media.githubusercontent.com/media/dav-sea/EasyTab/master/Media/et-border.png)

`Escape` (default):
 The solver will go beyond the boundaries of this `Transform` and search in neighboring `Transform`\`s

`Roll`: The solver will **NOT** go beyond the boundaries of this Transform, but simply start navigation from the first available child element (👉 the best solution for pop-ups and windows)

`Clamp`: The solver will NOT go beyond the boundaries of this Transform, but simply stop at the last selected

#### ChildrenExtracting
You can exclude all the children of this Transform from the tree

#### JumpingPolicy
Allows you to set the behavior of jumping to other elements during navigation.

> [!TIP]
>
>  The specified jumps will be used only when the definition of the next `Selectable` starts with this `GameObject` (that is, when it is specified as `EventSystem.current.currentSelectedGameObject`). When traversing the `Transform` tree, jumps will be ignored

`DontUseJumps`: Disables jumping

`UseOnlyJumps`:  When navigating from this `GameObject`, the specified jumps will be used to determine the next `Selectable`. But if the specified jumps are unavailable (for example, not interactable), then the transition to the next `Selectable` will not occur.

`UseJumpsOrHierarchy`: When navigating from this `GameObject`, the specified jumps will be used to determine the next `Selectable`. But if the specified jumps are unavailable (for example, not interactable), then the next `Selectable` will be searched by traversing the `Transform` tree relative to this `GameObject`.

`UseJumpsOrTheirNext`: When navigating from this `GameObject`, the specified jumps will be used to determine the next `Selectable`. But if the specified jumps are unavailable (for example, not interactable), then the process of finding for the next `Selectable` will be started again, but relative `GameObject` specified as a jump (if it is not destroyed).

#### NextJump & ReverseJump
Jumps to the next and previous `GameObject` to be used when navigating from this `GameObject`. (Jumps are used only when `JumpingPolicy` is not `DontUseJumps`)

#### NavigationLock
You can block navigation when this `GameObject` is current. This can be useful when you don't need navigation on this `GameObject`. In Auto mode, navigation is blocked automatically when this `GameObject` contains an `InputField` in the multiline and focused state

#### EnterHandling
You can determine whether navigation on this `GameObject` should occur when you press the <kbd>Enter</kbd> key. In auto mode, when you press <kbd>Enter</kbd>, navigation will occur if this `GameObject` contains an `InputField` in the single-line state

### Configuration via `EasyTabIntegration` class
`EasyTabIntegration` is a class that uses `InputSystem` and `EventSystem` to define the input and the currently selected object. As well as processing auxiliary functions, such as remembering the last selected object.

For ease of integration, one instance of class `EasyTabIntegration` is located in `EasyTabIntegration.Globally`. The update logic of this instance is embedded in `PlayerLoop`, and is always processed.    

If this does not suit you, you can disable the processing of the global object from `PlayerLoop` via `EasyTabIntegration.GloballyEnabled = false`

You can call the update logic yourself at the time you need via `EasyTabIntegration.Globally.updateAll()`. 

Or you can make your own instance of `EasyTabIntegration` and work with it, ignoring `EasyTabIntegration.Globally`

You can also configure `EasyTabIntegration.Solver` (see the following section)

### Configuration `EasyTabSolver`
`EasyTabSolver` is a class whose task is to decide which `Selectable` will be next. To configure it globally, use `EasyTabIntegration.Globally.Solver`

---
#### EntryPoint
The `GameObject` to be used as the initial one. Combined with the `FallbackNavigationPolicy` specified in the `WhenCurrentIsNotSet` and `WhenCurrentIsNotSelectable` fields

#### LastSelected
The last selected object. If this solver is specified in `EasyTabIntegration`, then this field is updated by it. Combined with the `FallbackNavigationPolicy` specified in the `WhenCurrentIsNotSet` and `WhenCurrentIsNotSelectable` fields

#### FallbackNavigationPolicy
You can set `NavigationPolicy` in different cases: `WhenCurrentIsNotSet`, `WhenCurrentIsNotSelectable`

`AllowNavigateToEntryPoint`: Allow navigation on `EntryPoint` as a fallback option

`AllowNavigateToClosest`: Allow to find the nearest `Selectable` (works only if the current `GameObject` is not null) as a fallback option

`AllowNavigateToLastSelected`: Allow navigation to the last selected object (especially useful when focus is lost when clicking into the void) as a fallback option

# Integration with your tools
The idea of EasyTab is that it works out of the box and does not require any configuration. However, if you use third-party tools, for example, to hide UI elements and windows, you need to define a small bridge between this intranet and EasyTab for correct works

A common practice when developing UI in Unity is to hide elements by changing transparency, moving outside the screen space, reducing scale, etc.

As a rule, there is always some bool property that determines whether the object is visible or not.
In this case, you need to add a simple condition so that EasyTab can take this into account

For example, like this:

```c#
var globalDrivers = EasyTabIntegration.Globally.Solver.Drivers;
var conditions = globalDrivers.Conditions.ConditionsForTraversingChildren;

// It is necessary to define a condition in case of non-met of which the children of this transform will not be traversed.
conditions.Add(t => !t.TryGetComponent<Window>(out var window) || window.IsActive);
```

It can also be useful to exclude specific elements, for this you should use `ConditionsForSelectable` instead of `ConditionsForTraversingChildren`

```c#
var globalDrivers = EasyTabIntegration.Globally.Solver.Drivers;
var conditions = globalDrivers.Conditions.ConditionsForSelectable;

// We exclude transparent elements
conditions.Add(t => !t.TryGetComponent<Graphic>(out var graphic) || graphic.color.a > 0);
```