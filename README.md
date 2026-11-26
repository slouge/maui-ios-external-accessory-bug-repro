# Maui.Bluetooth.ExternalAccessory Bug Reproduction

This repository contains two minimal .NET MAUI iOS applications that reproduce a bug related to `EAAccessoryManager.ShowBluetoothAccessoryPicker` using the Brother Mobile SDK's External Accessory protocol.

## Projects

| Project | .NET MAUI | Target Framework | iOS SDK |
|---|---|---|---|
| `Maui10ExternalAccessoryRepro` | MAUI 10 | `net10.0-ios` | iOS 26.5 SDK (Xcode 26.6) |
| `Maui10NoSceneDelegateExternalAccessoryRepro` | MAUI 10 | `net10.0-ios` | iOS 26.5 SDK (Xcode 26.6) |

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

`Maui10NoSceneDelegateExternalAccessoryRepro` retains the traditional `UIApplicationDelegate`-only lifecycle.

## Info.plist Configuration

Both projects are configured per the [Brother Mobile SDK iOS Getting Started guide](https://support.brother.com/g/s/es/htmldoc/mobilesdk/guide/getting-started/getting-started-ios.html):

- `NSBluetoothAlwaysUsageDescription`
- `NSBluetoothPeripheralUsageDescription`
- `UISupportedExternalAccessoryProtocols` → `com.brother.ptcbp`
- `NSLocalNetworkUsageDescription`
- `NSBonjourServices` → `_pdl-datastream._tcp`, `_printer._tcp`, `_ipp._tcp`

## Prerequisites

- macOS 26.2+ with **Xcode 26.6** (the version required by the .NET 10 iOS workload 26.5.10318)
- .NET **10.0.404** SDK (runtime 10.0.15) with the `maui-ios` workload set **10.0.401** (iOS 26.5.10318). Newer workload sets (10.0.401.1+) target Xcode 27 and will fail with Xcode 26.6

Install workloads:
```bash
dotnet workload install maui-ios --version 10.0.401
```

## Building

Open `Maui.Bluetooth.ExternalAccessory.slnx` in Visual Studio / Rider, select the desired project, and deploy to an iOS device or simulator.

Or via CLI:
```bash
dotnet build Maui10ExternalAccessoryRepro/Maui10ExternalAccessoryRepro.csproj -f net10.0-ios
dotnet build Maui10NoSceneDelegateExternalAccessoryRepro/Maui10NoSceneDelegateExternalAccessoryRepro.csproj -f net10.0-ios
```
