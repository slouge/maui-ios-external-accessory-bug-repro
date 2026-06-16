# Maui.Bluetooth.ExternalAccessory Bug Reproduction

This repository contains two minimal .NET MAUI iOS applications that reproduce a bug related to `EAAccessoryManager.ShowBluetoothAccessoryPicker` using the Brother Mobile SDK's External Accessory protocol.

## Projects

| Project | .NET MAUI | Target Framework | iOS SDK |
|---|---|---|---|
| `Maui9ExternalAccessoryRepro` | MAUI 9 | `net9.0-ios` | iOS 18 (Xcode 16) |
| `Maui10ExternalAccessoryRepro` | MAUI 10 | `net10.0-ios` | iOS 26 (Xcode 26) |

The source code is identical between both projects — only the `.csproj` target framework and SDK version differ.

## Application Behavior

Each app contains a single page with a **"Show Bluetooth Accessory Picker"** button bound to a `ShowBluetoothAccessoryPickerCommand` in `MainViewModel`.

Tapping the button calls:
```csharp
EAAccessoryManager.SharedAccessoryManager.ShowBluetoothAccessoryPicker(
    new NSDictionary(), HandleBluetoothPickerCompletion);
```

Logs are emitted with the prefix `[ExternalAccessoryRepro]` and are visible in the **macOS Console** app.

## iOS Scene Lifecycle (MAUI 10 only)

`Maui10ExternalAccessoryRepro` is configured to use the modern iOS `UISceneDelegate` lifecycle:

- `SceneDelegate.cs` — `Platforms/iOS/SceneDelegate.cs` subclasses `MauiUISceneDelegate`
- `AppDelegate.cs` — overrides `GetConfiguration` to return a `UISceneConfiguration` named `"Default Configuration"`
- `Info.plist` — declares `UIApplicationSceneManifest` with `UISceneDelegateClassName` pointing to `SceneDelegate`

`Maui9ExternalAccessoryRepro` retains the traditional `UIApplicationDelegate`-only lifecycle.

## Info.plist Configuration

Both projects are configured per the [Brother Mobile SDK iOS Getting Started guide](https://support.brother.com/g/s/es/htmldoc/mobilesdk/guide/getting-started/getting-started-ios.html):

- `NSBluetoothAlwaysUsageDescription`
- `NSBluetoothPeripheralUsageDescription`
- `UISupportedExternalAccessoryProtocols` → `com.brother.ptcbp`
- `NSLocalNetworkUsageDescription`
- `NSBonjourServices` → `_pdl-datastream._tcp`, `_printer._tcp`, `_ipp._tcp`

## Prerequisites

- macOS with **Xcode 16** (for MAUI 9 / iOS 18) or **Xcode 26** (for MAUI 10 / iOS 26)
- .NET 9 SDK with `maui-ios` workload (for `Maui9ExternalAccessoryRepro`)
- .NET 10 SDK with `maui-ios` workload (for `Maui10ExternalAccessoryRepro`)

Install workloads:
```bash
dotnet workload install maui-ios
```

## Building

Open `Maui.Bluetooth.ExternalAccessory.slnx` in Visual Studio / Rider, select the desired project, and deploy to an iOS device or simulator.

Or via CLI:
```bash
dotnet build Maui9ExternalAccessoryRepro/Maui9ExternalAccessoryRepro.csproj -f net9.0-ios
dotnet build Maui10ExternalAccessoryRepro/Maui10ExternalAccessoryRepro.csproj -f net10.0-ios
```
