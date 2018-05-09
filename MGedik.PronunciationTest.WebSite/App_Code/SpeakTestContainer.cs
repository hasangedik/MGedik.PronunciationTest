using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGedik.PronunciationTest.WebSite.Models;

namespace MGedik.PronunciationTest.WebSite
{
    public static class SpeakTestContainer
    {
        private static readonly List<SpeakTestItem> SpeakTestItems = new List<SpeakTestItem>();

        static SpeakTestContainer()
        {
            SpeakTestItems.Add(new SpeakTestItem { Id = 1, Question = "Hi, what is your name?", Answer = "my name is|i am|i'm|hello my name is|hello i am|hello i'm" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 2, Question = "How are you?", Answer = "fine|i am fine|i'm fine|i am great|i'm great|i am tired|i'm tired|i am sleepy|i'm sleepy|i am moody|i'm moody|i am bad|i'm bad|i am good|i'm good|i am not good|i'm not good" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 3, Question = "How old are you?", Answer = "i am|i'm" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 4, Question = "Where are you from?", Answer = "i am from|i'm from" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 5, Question = "What are your hobbies?", Answer = "my hobbies are" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 6, Question = "What time do you have breakfast?", Answer = "i have breakfast at" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 7, Question = "When is your birthday?", Answer = "my birthday is on the" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 8, Question = "What is your sunsign?", Answer = "i am a|i'm a" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 9, Question = "What is your father's job?", Answer = "my father is|he is" });
            SpeakTestItems.Add(new SpeakTestItem { Id = 10, Question = "What is your mother's job?", Answer = "my mother is|she is" });
        }

        public static SpeakTestItem GetNextSpeakTestItem()
        {
            var nextSpeakTestItem = HttpContext.Current.Session["nextSpeakTestItem"] as SpeakTestItem;
            if (nextSpeakTestItem != null)
            {
                if (SpeakTestItems.Count(x => x.IsSucceed.HasValue && x.IsSucceed.Value) == SpeakTestItems.Count)
                    return null;

                if (!SpeakTestItems.First(x => x.Id == nextSpeakTestItem.Id).IsSucceed.HasValue)
                    return nextSpeakTestItem;

                SpeakTestItem item;
                int nextIndex = SpeakTestItems.IndexOf(nextSpeakTestItem) + 1;
                if (nextIndex < SpeakTestItems.Count)
                    item = SpeakTestItems[nextIndex];
                else
                    item = SpeakTestItems[0];

                HttpContext.Current.Session["nextSpeakTestItem"] = item;
                if (item.IsSucceed.HasValue && item.IsSucceed.Value && SpeakTestItems.Any(x=> !x.IsSucceed.HasValue || x.IsSucceed.Value == false))
                    return GetNextSpeakTestItem();
                return item;
            }
            else
            {
                var item = SpeakTestItems[0];
                HttpContext.Current.Session["nextSpeakTestItem"] = item;
                return item;
            }
        }

        public static void SetSpeakTestItemStatus(int id, bool status)
        {
            SpeakTestItems.First(x => x.Id == id).IsSucceed = status;
        }
    }
}