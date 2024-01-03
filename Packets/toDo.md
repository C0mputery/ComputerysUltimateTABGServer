# These are every instance the tabg client sends a packet to the server. 

## SendCode | Class Name | Status

### In Progress:
```
SendMessageToServer(EventCode.ThrowChatMessage, array, reliable: true); | ClientRequestTalkingRockThrow | Not Implemented
SendMessageToServer(EventCode.PlayerMarkerAdded, array, reliable: true); | ClientAddMarker | Not Implemented
SendMessageToServer(EventCode.PlayerMarkerAdded, array, reliable: true); | ClientRemoveMarker | Not Implemented

SendMessageToServer(EventCode.WeaponChange, array, reliable: true); | ClientChangedWeapon | Not Implemented
SendMessageToServer(EventCode.PlayerFire, array, reliable: true); | SendPlayerFireUpdate | Not Implemented

SendMessageToServer(EventCode.RequestItemDrop, array, reliable: true); | ClientRequestDrop | Not Implemented
SendMessageToServer(EventCode.RequestWeaponPickUp, array, reliable: true); | ClientRequestPickUp | Not Implemented
SendMessageToServer(EventCode.RequestItemThrow, array, reliable: true); | ClientRequestThrow | Not Implemented

SendMessageToServer(EventCode.PlayerDead, array, reliable: true); | KillLocalPlayer | Not Implemented
SendMessageToServer(EventCode.RingDeath, data, reliable: true); | OnRingDeath | Not Implemented
```
### To Do:
```
SendMessageToServer(EventCode.ACRequestedData, data, reliable: true); | ClientSendACDataChannelRequest | Not Implemented
SendMessageToServer(EventCode.NetworkPlayerTransmittedPackage, array, reliable: true); | SendNetworkPlayerTransmittedPackage | Not Implemented
SendMessageToServer(EventCode.BossFightResult, array, reliable: true); | SendBossResultToServer | Not Implemented
SendMessageToServer(EventCode.RequestSyncProjectileEvent, array2, reliable: true); | ClientRequestProjectileSyncEvent | Not Implemented
SendMessageToServer(EventCode.RequestCurseCleanse, data, reliable: true); | ClientRequestCleanseCurse | Not Implemented
SendMessageToServer(EventCode.RequestBlessing, array, reliable: true); | ClientRequestChangeBlessings | Not Implemented
SendMessageToServer(EventCode.RequestBlessing, data, reliable: true); | ClientRequestSummonBlessing | Not Implemented
SendMessageToServer(EventCode.RequestRespawnTeamMate, array, reliable: true); | ClientRequestRespawnTeamMate | Not Implemented
SendMessageToServer(EventCode.RequestClickInteract, array, reliable: true); | RequestClickInteract | Not Implemented
SendMessageToServer(EventCode.RequestHealthState, array, reliable: true); | RequestNewHealthState | Not Implemented
SendMessageToServer(EventCode.SpectatorRequest, data, reliable: true); | RequestNewSpectator | Not Implemented
SendMessageToServer(EventCode.PassangerUpdate, array, reliable: false); | SendPassangerUpdate | Not Implemented
SendMessageToServer(EventCode.PlayerEffect, array, reliable: true); | ClientDoMiscEffect | Not Implemented
SendMessageToServer(EventCode.PlayerEffect, array, reliable: true); | ClientDoMiscEffectVector3 | Not Implemented
SendMessageToServer(EventCode.PlayerEffect, array, reliable: true); | ClientDoEffect | Not Implemented
SendMessageToServer(EventCode.PlayerState, data, reliable: true); | SendLocalPlayerState | Not Implemented
SendMessageToServer(EventCode.ReviveState, array, reliable: true); | StartReviving | Not Implemented
SendMessageToServer(EventCode.ReviveState, array, reliable: true); | StopReviving | Not Implemented
SendMessageToServer(EventCode.ReviveState, array, reliable: true); | FinishedReviving | Not Implemented
SendMessageToServer(EventCode.CarDamage, array, reliable: true); | ClientDamageCar | Not Implemented
SendMessageToServer(EventCode.CarTemporaryUpdate, array2, reliable: false); | SendCarTempUpdate | Not Implemented // erm wtf is a temp update???
SendMessageToServer(EventCode.CarUpdate, array3, reliable: false); | SendCarUpdate | Not Implemented
SendMessageToServer(EventCode.RequestHealing, array, reliable: true); | ClientRequestHeal | Not Implemented
SendMessageToServer(EventCode.RequestAirplaneDrop, array, reliable: true); | ClientRequestPlaneDrop | Not Implemented
SendMessageToServer(EventCode.PlayerLand, array, reliable: true); | ClientSkydivingLand | Not Implemented
SendMessageToServer(EventCode.RequestSeat, array, reliable: true); | ClientRequestSeat | Not Implemented
SendMessageToServer(EventCode.AdminCommand, array, reliable: true); | SendAdminCommand | Not Implemented
SendMessageToServer(EventCode.RequestDefuseBomb, new byte[0], reliable: true); | RequestDefuse | Not Implemented
SendMessageToServer(EventCode.RequestPlantBomb, array, reliable: true); | RequestPlantBomb | Not Implemented
SendMessageToServer(EventCode.RequestStopDefuse, new byte[0], reliable: true); | RequestStopDefuse | Not Implemented
SendMessageToServer(EventCode.RequestPurchaseGun, data, reliable: true); | RequestBuyWeapon | Not Implemented
```
### Done:
```
SendMessageToServer(EventCode.SendCatchPhrase, new byte[0], reliable: true); | SendCatchPhrase | Implemented
SendMessageToServer(EventCode.DamageEvent, array, reliable: true); | DamageEntity | Implemented
SendMessageToServer(EventCode.RequestWorldState, data, reliable: true); | RequestInit | Implemented
SendMessageToServer(EventCode.ChatMessage, array, reliable: true); | SendChatMessage | Implemented
SendMessageToServer(EventCode.GearChange, array, reliable: true); | ClientChangeGear | Implemented
SendMessageToServer(EventCode.TABGPing, bytes, reliable: true); | PingUnityServer | Implemented
SendMessageToServer(EventCode.PlayerUpdate, array2, reliable: false); | SendPlayerUpdate | Implemented
```

