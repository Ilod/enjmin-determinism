using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameState
{
    public struct ArrowState
    {
        public float x;
        public float y;
        public int player;
    }

    public struct PlayerState
    {
        public float x;
        public float y;
        public int index;
        public int score;
        public Color color;
        public bool invincible;
        public bool dead;
    }

    public List<ArrowState> arrowStates = new List<ArrowState>();
    public List<PlayerState> playerStates = new List<PlayerState>();

    public static GameState ComputeGameState()
    {
        var state = new GameState();
        foreach (var player in GameObject.FindGameObjectsWithTag("Player")
            .OrderBy(p => p.GetComponent<PlayerAI>().index))
        {
            var playerAi = player.GetComponent<PlayerAI>();
            var playerState = new PlayerState();
            playerState.x = player.transform.position.x;
            playerState.y = player.transform.position.y;
            playerState.index = playerAi.index;
            playerState.score = playerAi.score;
            playerState.color = playerAi.color;
            playerState.invincible = playerAi.invincibilityRemaining > 0;
            playerState.dead = playerAi.dead;
            state.playerStates.Add(playerState);
        }
        foreach (var arrow in GameObject.FindGameObjectsWithTag("Arrow")
            .OrderBy(a => a.GetComponent<ArrowAI>().player.index)
            .ThenBy(a => a.transform.position.x)
            .ThenBy(a => a.transform.position.y))
        {
            var arrowState = new ArrowState();
            arrowState.x = arrow.transform.position.x;
            arrowState.y = arrow.transform.position.y;
            arrowState.player = arrow.GetComponent<ArrowAI>().player.index;
            state.arrowStates.Add(arrowState);
        }
        return state;
    }

    public static GameState ReadFromPacket(Packet packet)
    {
        GameState state = new GameState();
        var arrowCount = packet.ReadInt();
        for (int i = 0; i < arrowCount; ++i)
        {
            var arrow = new ArrowState();
            arrow.x = packet.ReadFloat();
            arrow.y = packet.ReadFloat();
            arrow.player = packet.ReadInt();
            state.arrowStates.Add(arrow);
        }
        var playerCount = packet.ReadInt();
        for (int i = 0; i < playerCount; ++i)
        {
            var player = new PlayerState();
            player.x = packet.ReadFloat();
            player.y = packet.ReadFloat();
            player.index = packet.ReadInt();
            player.score = packet.ReadInt();
            player.color = new Color();
            player.color.r = packet.ReadFloat();
            player.color.g = packet.ReadFloat();
            player.color.b = packet.ReadFloat();
            player.invincible = packet.ReadBool();
            player.dead = packet.ReadBool();
            state.playerStates.Add(player);
        }
        return state;
    }

    public void WriteInPacket(PacketBuilder builder)
    {
        builder.Write(arrowStates.Count);
        foreach (var arrow in arrowStates)
        {
            builder.Write(arrow.x);
            builder.Write(arrow.y);
            builder.Write(arrow.player);
        }
        builder.Write(playerStates.Count);
        foreach (var player in playerStates)
        {
            builder.Write(player.x);
            builder.Write(player.y);
            builder.Write(player.index);
            builder.Write(player.score);
            builder.Write(player.color.r);
            builder.Write(player.color.g);
            builder.Write(player.color.b);
            builder.Write(player.invincible);
            builder.Write(player.dead);
        }
    }

    public Packet ComputePacket()
    {
        var builder = new PacketBuilder();
        WriteInPacket(builder);
        return builder.Build();
    }

    public int ComputeHash()
    {
        var data = ComputePacket().Data;
        unchecked
        {
            const int p = 16777619;
            int hash = (int)2166136261;

            for (int i = 0; i < data.Length; i++)
                hash = (hash ^ data[i]) * p;

            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;
            return hash;
        }
    }

    public override int GetHashCode()
    {
        return ComputeHash();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"{arrowStates.Count} arrows");
        foreach (var arrow in arrowStates)
            builder.AppendLine($"Arrow player {arrow.player}, ({arrow.x},{arrow.y})");
        builder.AppendLine();
        foreach (var player in playerStates)
            builder.AppendLine($"Player {player.index} with {player.score} ({player.x},{player.y}), {(player.invincible ? "invincible" : "")} {(player.dead ? "dead" : "")} rgb={player.color.r},{player.color.g},{player.color.b})");
        return builder.ToString();
    }
}
