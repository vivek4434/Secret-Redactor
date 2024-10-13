using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingWindow
{
    public class Window
    {
        private const int MAX = 1000000;
        
        public int startingIndex {  get; set; }

        public int endingIndex {  get; set; }

        public int nextStartingIndex { get; set; }

        public int nextEndingIndex { get; set; }

        public Window() 
        { 
            startingIndex = -1;
            endingIndex = 0;
            nextStartingIndex = MAX;
            nextEndingIndex = MAX;
        }

        public Window(int x, int y) 
        { 
            startingIndex = x;
            endingIndex = y;
            nextStartingIndex = MAX;
            nextEndingIndex = MAX;
        }

        public int WindowLength
        {
            get
            {
                return endingIndex - startingIndex;
            }
        }

        public bool ContainsOverlap()
        {
            return endingIndex >= nextStartingIndex;
        }
    }
}
