using ComputerysUltimateTABGServer.Rooms;
using System.Text;
using System.Text.Json;

namespace ComputerysUltimateTABGServer.TabgServerList
{
    public static class ServerListManager
    {
        public static readonly HttpClient httpClient = new HttpClient();

        private static string? m_ExternalIP;
        private static Timer? heartbeatTimer;

        public static void StartServerListHeartbeat()
        {
            m_ExternalIP = GetExternalIpAddress().Result;
            heartbeatTimer = new Timer(ServerListHeartbeat, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private static async void ServerListHeartbeat(object? state)
        {
            foreach (Room room in RoomManager.ActiveRooms.Values)
            {
                var serverInfo = new
                {
                    joinCode = $"{m_ExternalIP}:{room.m_EnetAddress.Port}",
                    maxPlayers = room.m_MaxClients,
                    serverName = room.m_RoomName,
                    playersOnServer = room.m_Players.Count(),
                    version = "693fa81cb14a5bc46bf18456d161a7c6", // I bet we can change this to whatever we want so modded servers can have there own list!
                    description = "testing cuts", // have this defined by room
                    password = "", // have this defined by room
                    gameMode = "testing", // have this defined by room
                    acceptingPlayers = true, // have this defined by room
                    serverType = "dedicated", // have this defined by room
                    squadMode = "n/a", // have this defined by room
                };

                await HTTPJsonPostCall("https://tabgcommunitybackend.azurewebsites.net/GameServerHeartbeat", JsonSerializer.Serialize(serverInfo));
            }
        }

        public static async Task<string?> HTTPJsonPostCall(string fullUrl, string jsonBody)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(fullUrl, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"HTTP error: {response.StatusCode}");
                }
            }
            catch (Exception)
            {
                return "Failed to send HTTP POST request";
            }
        }

        public static async Task<string?> GetExternalIpAddress()
        {
            return (await httpClient.GetStringAsync("http://icanhazip.com")).Replace("\\r\\n", "").Replace("\\n", "").Trim();
            // We should probably add to the config file a way to set the ip address.
        }
    }
}
