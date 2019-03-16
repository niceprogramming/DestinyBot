namespace DestinyBot.Data.Entities
{
    public class CustomCommand
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string[] Aliases { get; set; }
    }
}