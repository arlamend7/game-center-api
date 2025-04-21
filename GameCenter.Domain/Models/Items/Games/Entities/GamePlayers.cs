namespace GameCenter.Domain.Models.Games.Entities
{
    public class GamePlayers
    {
        public int Min {  get; set; }
        public int Max { get; set; }

        public static GamePlayers SinglePlayer = new GamePlayers(1,1);
        public GamePlayers(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

}
