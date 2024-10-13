using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlidingWindow
{
    internal class SlidingWindowRedaction
    {
        public WindowList RedactionWindowList;

        public SlidingWindowRedaction()
        {
            RedactionWindowList = new WindowList();
        }

        public SlidingWindowRedaction(string inputString)
        {
            RedactionWindowList = new WindowList();
        }

        public List<Window> GetRedactionWindowList(string inputString)
        {
            PopulateRedactionWindows(inputString);
            return RedactionWindowList.GetRedactionList();
        }

        private WindowList PopulateRedactionWindows(string inputString)
        {
            RedactionWindowList.Clean();

            int startingIndexIterator = -1;
            int endingIndexIterator = 0;

            while(startingIndexIterator < inputString.Length)
            {
                while(endingIndexIterator < inputString.Length)
                {
                    // if hash match increase endingIdexIterator

                    // else add the window till now and break

                }

                startingIndexIterator = endingIndexIterator - 1;
            }

            return RedactionWindowList;
        }
    }
}
