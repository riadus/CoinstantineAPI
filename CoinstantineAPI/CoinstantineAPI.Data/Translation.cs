namespace CoinstantineAPI.Data
{
    public class Translation : Entity
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
        public string Comments { get; set; }
    }

    public class Country
    {
        public string Flag { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
