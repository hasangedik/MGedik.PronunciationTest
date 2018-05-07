namespace MGedik.PronunciationTest.WebSite.Models
{
    public class SpeakTestItem
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? IsSucceed { get; set; }
    }
}