
using System;
using UnityEngine;

public static class AuthenticationDtoMappers
{
    public static PlayerProfile FromDto(this PlayerProfileResponseDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        if (string.IsNullOrWhiteSpace(dto.displayName)) throw new FormatException(nameof(dto.displayName));

        if (!Guid.TryParse(dto.playerId, out Guid guid)) throw new FormatException(nameof(dto.playerId));

        if (!ColorUtility.TryParseHtmlString(dto.playerColor, out Color color)) throw new FormatException(nameof(dto.playerColor));

        PlayerId playerId = new(guid);
        Color32 color32 = color;
        PlayerColor playerColor = new(color32.r, color32.g, color32.b);
        return new PlayerProfile(playerId, dto.displayName, playerColor);
    }
}
