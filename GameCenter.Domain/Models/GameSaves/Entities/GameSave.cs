using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Players.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCenter.Domain.Models.GameSaves.Entities
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(GamePersistentSave), typeof(GameSoloSave), typeof(GameCoopSave), typeof(GameTournamentPointsSave), typeof(GameTournamentSave))]
    public class GameSave : EntityBase
    {
        public Guid GameId { get; set; }
        public List<UniqueUserInfo> Users { get; set; }
        public Dictionary<string, object> Options { get; set; }
        public PlayTypeEnum PlayType { get; protected set; }
        public SessionTypeEnum SessionType { get; protected set; }
    }

    public class GamePersistentSave : GameSave
    {
        public Translation<string> CheckPointName { get; set; }
        public object Save { get; set; }
    }

    public class GameSoloSave : GameSave
    {
        public GameSoloSave()
        {
            PlayType = PlayTypeEnum.Solo;
            SessionType = SessionTypeEnum.Single;
        }

        public bool Completed { get; set; }
        public GameMatch Match { get; set; }
    }

    public abstract class GameMultiplayerSave : GameSave
    {
        public bool HasBot => Bots.Any(x => x.Id == null);
        public List<UniqueUserInfo> Bots { get; set; }
    }
    public class GameCoopSave : GameMultiplayerSave
    {
        public GameMultiplayerMatch Match { get; set; }

        public GameCoopSave()
        {
            PlayType = PlayTypeEnum.Coop;
            SessionType = SessionTypeEnum.Single;
        }
    }

    public class GameTournamentPointsSave : GameMultiplayerSave
    {
        public GameRounds Rounds { get; set; }

        public GameTournamentPointsSave()
        {
            PlayType = PlayTypeEnum.tournamentPoints;
            SessionType = SessionTypeEnum.Single;
        }
    }
    public class GameTournamentSave : GameMultiplayerSave
    {
        public List<GameRound> Rounds { get; set; }
        public int MatchSize { get; set; }  
        public int MaxDepth { get; set; }

        public GameTournamentSave()
        {
            PlayType = PlayTypeEnum.tournament;
            SessionType = SessionTypeEnum.Single;
        }
    }

    public class GameRound : EntityBase
    {
        public Guid Parent {  get; set; }
        public List<Guid> Children { get; set; }
        public GameMultiplayerMatch Match { get; set; }
        public int Depth {  get; set; }
    }

    public class GameRounds: Dictionary<int, List<GameMatch>>
    {

    }
}
