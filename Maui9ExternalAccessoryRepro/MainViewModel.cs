using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

#if IOS
using ExternalAccessory;
#endif

namespace ExternalAccessoryRepro;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowBluetoothAccessoryPicker()
    {
        Log("ShowBluetoothAccessoryPicker: button tapped");

#if IOS
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Log("ShowBluetoothAccessoryPicker: invoking EAAccessoryManager picker");
            EAAccessoryManager.SharedAccessoryManager.ShowBluetoothAccessoryPicker(
                nameFilter: null,
                completion: (error) =>
                {
                    if (error is not null)
                        Log($"ShowBluetoothAccessoryPicker error: {error.LocalizedDescription}");
                    else
                        Log("ShowBluetoothAccessoryPicker: completed successfully");
                });
        });
#else
        Log("ShowBluetoothAccessoryPicker: not supported on this platform");
#endif
    }

    private static void Log(string message)
    {
        var line = $"[ExternalAccessoryRepro] {message}";
        Console.WriteLine(line);
        System.Diagnostics.Debug.WriteLine(line);
    }
}
