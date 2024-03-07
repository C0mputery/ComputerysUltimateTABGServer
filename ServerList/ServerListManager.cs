using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ComputerysUltimateTABGServer.TabgServerList
{
    public static class ServerListManager
    {
        public static readonly HttpClient httpClient = new HttpClient();

        private static string? m_ExternalIP;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "It's a timer and it's used until the end of the program.")]
        private static Timer? heartbeatTimer;

        public static void StartServerListHeartbeat()
        {
            m_ExternalIP = GetExternalIpAddress().Result;

            // Perhaps move this out into the room manager, so it can be called when a room is made.
            // Ideally we should be allowed to start a room later on not just at the start of the program.
            string allowedWordsPage = httpClient.GetStringAsync("https://raw.githubusercontent.com/landfallgames/tabg-word-list/main/all_words.txt").Result;
            string[] allowedWordsArray = allowedWordsPage.Split("\n");
            HashSet<string> allowedWordsHashset = new HashSet<string>(allowedWordsArray);
            foreach (Room room in RoomManager.ActiveRooms.Values)
            {
                // Find a more performant way of doing this, as it will be needed to be done every time a room is made and on every string.
                // Description, gameMode, squadmode, not just the name.
                foreach (string word in allowedWordsHashset)
                {
                    if (room.m_RoomName.Contains(word))
                    {
                        StringBuilder randomWords = new();
                        for (int i = 0; i < 3; i++)
                        {
                            int randomIndex = Misc.RandomNumber.Next(0, allowedWordsArray.Length);
                            randomWords.AppendJoin(" ", allowedWordsArray[randomIndex]);

                        }
                        string combinedWords = randomWords.ToString();
                        CUTSLogger.Log($"Room name: {room.m_RoomName} contains disallowed word, Renaming too: {combinedWords}", LogLevel.Error);
                        room.m_RoomName = combinedWords;
                        break;
                    }
                }
            }

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
                    serverType = "dedicated",
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
