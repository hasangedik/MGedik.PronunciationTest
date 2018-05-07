namespace MGedik.PronunciationTest.WebSite.Models
{
    public class TestItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Pronunciation { get; set; }
        public bool? IsSucceed { get; set; }
    }
}