namespace DarknessAwaits_API.Models
{
    public class Game
    {
        public int id { get; set; }
        public int user { get; set; }
        public int trys { get; set; }
        public int miliseconds { get; set; }
        public bool complete{ get; set; } = false;
        public string date { get; set; } = string.Empty;
    }
}
