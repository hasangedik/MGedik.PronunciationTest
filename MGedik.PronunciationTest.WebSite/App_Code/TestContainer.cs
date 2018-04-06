using System;
using System.Collections.Generic;
using System.Web;
using MGedik.PronunciationTest.WebSite.Models;

namespace MGedik.PronunciationTest.WebSite
{
    public static class TestContainer
    {
        private static readonly List<TestItem> TestItems = new List<TestItem>();

        static TestContainer()
        {
            TestItems.Add(new TestItem { Id = 1, Title = "Wash the dishes", Pronunciation = "wash the dishes", ImagePath = "/Content/test/washdishes.jpg" });
            TestItems.Add(new TestItem { Id = 2, Title = "Watch tv", Pronunciation = "watch tv", ImagePath = "/Content/test/watchtv.jpg" });
            TestItems.Add(new TestItem { Id = 3, Title = "Fly a kite", Pronunciation = "fly a kite", ImagePath = "/Content/test/flyakite.jpg" });
            TestItems.Add(new TestItem { Id = 4, Title = "Listen to teacher", Pronunciation = "listen to teacher", ImagePath = "/Content/test/listentoteacher.jpg" });
            TestItems.Add(new TestItem { Id = 5, Title = "Sweep the floor", Pronunciation = "sweep the floor", ImagePath = "/Content/test/sweepthefloor.jpg" });
            TestItems.Add(new TestItem { Id = 6, Title = "Climb the tree", Pronunciation = "climb the tree", ImagePath = "/Content/test/climbthetree.jpg" });
            TestItems.Add(new TestItem { Id = 7, Title = "Do exercise", Pronunciation = "do exercise", ImagePath = "/Content/test/doexercise.jpg" });
            TestItems.Add(new TestItem { Id = 8, Title = "Prepare breakfast", Pronunciation = "prepare breakfast", ImagePath = "/Content/test/preparebreakfast.jpg" });
            TestItems.Add(new TestItem { Id = 9, Title = "Have a rest", Pronunciation = "have a rest", ImagePath = "/Content/test/havearest.jpg" });
            TestItems.Add(new TestItem { Id = 10, Title = "Dust the shelves", Pronunciation = "dust the shelves", ImagePath = "/Content/test/dusttheshelves.jpg" });
            TestItems.Add(new TestItem { Id = 11, Title = "Take a nap", Pronunciation = "take a nap", ImagePath = "/Content/test/takeanap.jpg" });
            TestItems.Add(new TestItem { Id = 12, Title = "Get on well with", Pronunciation = "get on well with", ImagePath = "/Content/test/getonwellwith.jpg" });
            TestItems.Add(new TestItem { Id = 13, Title = "Travel abroad", Pronunciation = "travel abroad", ImagePath = "/Content/test/travelabroad.jpg" });
            TestItems.Add(new TestItem { Id = 14, Title = "Learn a language", Pronunciation = "learn a language", ImagePath = "/Content/test/learnalanguage.jpg" });
        }

        public static TestItem GetRandomTestItem()
        {
            Random rand = new Random();
            int size = TestItems.Count;
            return TestItems[rand.Next(size)];
        }

        public static TestItem GetNextTestItem()
        {
            var nextTestItem = HttpContext.Current.Session["nextTestItem"] as TestItem;
            if (nextTestItem != null)
            {
                TestItem item;
                int nextIndex = TestItems.IndexOf(nextTestItem) + 1;
                if (nextIndex < TestItems.Count)
                    item = TestItems[nextIndex];
                else
                    item = TestItems[0];

                HttpContext.Current.Session["nextTestItem"] = item;
                return item;
            }
            else
            {
                var item = TestItems[0];
                HttpContext.Current.Session["nextTestItem"] = item;
                return item;
            }
        }
    }
}