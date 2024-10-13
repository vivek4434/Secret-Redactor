using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingWindow
{
    internal class WindowList
    {
        public List<Window> Windows;

        public int minSecretSize {  get; set; }

        public WindowList()
        {
            Windows = new List<Window>();
            minSecretSize = 3;
        }

        public WindowList(List<Window> windows)
        {
            Windows = windows;
            minSecretSize = 3;
        }

        public void Clean()
        {
            Windows.Clear();
        }

        public List<Window> GetRedactionList()
        {
            FillWindowNextStartingIndex();

            foreach (Window window in Windows)
            {
                if (window.WindowLength < minSecretSize)
                {
                    Windows.Remove(window);
                }
            }

            List<Window> redactionList = CombineOverlappingWindows();

            return redactionList;
        }

        public void FillWindowNextStartingIndex()
        {
            int iterator = 0;
            while (iterator + 1 < Windows.Count())
            {
                Windows[iterator].nextStartingIndex = Windows[iterator + 1].startingIndex;
                Windows[iterator].nextEndingIndex = Windows[iterator + 1].endingIndex;
                iterator++;
            }
        }

        private List<Window> CombineOverlappingWindows()
        {
            List<Window> redactionList = new List<Window>();
            int iterator = 0;
            int startingIndex = Windows[iterator].startingIndex;
            int endingIndex = Windows[iterator].endingIndex;

            while (iterator < Windows.Count())
            {
                startingIndex = Windows[iterator].startingIndex;

                while (Windows[iterator].ContainsOverlap())
                {
                    endingIndex = Windows[iterator].nextEndingIndex;
                    iterator++;
                }

                Window newWindow = new Window(startingIndex, endingIndex);
                redactionList.Add(newWindow);

                iterator++;
            }

            return redactionList;
        }
    }
}
