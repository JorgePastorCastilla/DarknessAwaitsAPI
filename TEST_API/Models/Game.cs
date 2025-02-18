namespace DarknessAwaits_API.Models
{
    public class Game
    {
        public int id { get; set; }
        public int user { get; set; }
        public int trys { get; set; }
        public int miliseconds { get; set; }
        public int complete{ get; set; } = 0;
        public string date { get; set; } = string.Empty;
    }
}
