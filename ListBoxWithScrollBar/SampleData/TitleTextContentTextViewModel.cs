using System;
using System.Collections.Generic;

namespace ListBoxWithScrollBar.SampleData
{
    public class TitleTextContentText
    {
        public String title { get; set; }
        public String content { get; set; }
    }

    public class TitleTextContentTextViewModel
    {
        public TitleTextContentTextViewModel()
        {
            SampleSource = new List<TitleTextContentText>();
            addTestData();
        }

        public List<TitleTextContentText> SampleSource { get; set; }

        private void addTestData()
        {
            for (int i = 0; i < 1000; i++)
            {
                SampleSource.Add(new TitleTextContentText()
                {
                    title = "title" + i.ToString(),
                    content = "content" + i.ToString()
                });
            }
        }
    }
}
