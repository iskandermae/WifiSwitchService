using Windows.Devices.WiFi;

namespace HWork.WifiSwitchService {

    public sealed class WifiHelper {

        public static async void Switch(string allowed) {
            var allowedSSID = allowed.Trim().Split(';');
            IReadOnlyList<WiFiAdapter>? adapters = await WiFiAdapter.FindAllAdaptersAsync();
            if (adapters == null || adapters.Count == 0) {
                return;
            }

            byte strength = byte.MinValue;
            WiFiAvailableNetwork? powerfulNetwork = null;
            WiFiAdapter? powerfulAdapter = null;

            foreach (WiFiAdapter? adapter in adapters) {
                foreach (WiFiAvailableNetwork? network in adapter.NetworkReport.AvailableNetworks) {
                    if (network == null)
                        continue;
                    if (!allowedSSID.Contains(network.Ssid))
                        continue;
                    if (network.SignalBars <= strength) {
                        continue;
                    }
                    strength = network.SignalBars;
                    powerfulNetwork = network;
                    powerfulAdapter = adapter;
                }
            }
            if (powerfulNetwork == null || powerfulAdapter == null) {
                return;
            }
            await powerfulAdapter.ConnectAsync(powerfulNetwork, WiFiReconnectionKind.Automatic);
        }
    }
}
