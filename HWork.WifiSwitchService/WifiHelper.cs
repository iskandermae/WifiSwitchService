using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace HWork.WifiSwitchService {

    public sealed class WifiHelper {

        public static async Task<string> Switch(string allowed) {
            var allowedSSID = allowed.Trim().Split(';');

            IReadOnlyList<WiFiAdapter>? adapters = await WiFiAdapter.FindAllAdaptersAsync();
            if (adapters == null || adapters.Count == 0) {
                return "No adapters found";
            }

            byte strength = byte.MinValue;
            WiFiAvailableNetwork? powerfulNetwork = null;
            WiFiAdapter? powerfulAdapter = null;
            byte? signalBars = null;
            foreach (WiFiAdapter adapter in adapters) {
                ConnectionProfile report = await adapter.NetworkAdapter.GetConnectedProfileAsync();
                signalBars = report?.GetSignalBars();
                if (signalBars == 5) {
                    return "Current network is 5 Bars";
                }
                strength = signalBars ?? strength;
                foreach (WiFiAvailableNetwork? network in adapter.NetworkReport.AvailableNetworks) {
                    if (network == null)
                        continue;
                    if (!allowedSSID.Contains(network.Ssid))
                        continue;
                    if (network.SignalBars <= strength) {
                        continue;
                    }
                    if (network.SecuritySettings== null || !network.SecuritySettings.NetworkAuthenticationType.ToString().Contains("RsnaPsk")) {
                        continue;
                    }
                    strength = network.SignalBars;
                    powerfulNetwork = network;
                    powerfulAdapter = adapter;
                }
            }
            if (powerfulNetwork == null || powerfulAdapter == null) {
                return $"Current network is {signalBars} Bars. No better found.";
            }
            await powerfulAdapter.ConnectAsync(powerfulNetwork, WiFiReconnectionKind.Automatic);
            return $"Switched to {powerfulNetwork.Ssid}";
        }
    }
}
