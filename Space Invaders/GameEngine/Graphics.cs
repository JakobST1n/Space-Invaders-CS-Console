using System;
using System.Diagnostics;

namespace GameEngine {

    class Graphics {
        /* This class is made for drawing frame by frame in the console. 
         * It is designed to be used from a while loop, where the draw-frame method is called at the end of the loop */

        // IMPORTANT! You might have to change the console font size, depending on how many rows and columns you want to have!

        public int Rows { get; }          // This should not be editable
        public int Columns { get; }       // Neither should this
        public int FrameNum;              // How many frames have been drawn

        private Char[,] _consoleBuffer;   // This stores how the screen looked last frame
        private Stopwatch _stopWatch;     // This is used to calculate time between frames

        public Graphics(int columns, int rows) {
            Rows = rows;
            Columns = columns;
            _consoleBuffer = new Char[Columns, Rows];

            Console.CursorVisible = false;
            Console.WindowHeight = Columns;
            Console.WindowWidth = Rows;
            Console.SetBufferSize(rows, columns);

            _stopWatch = Stopwatch.StartNew();
            FrameNum = 0;
        }

        public void DrawFrame(Frame newFrame) {
            /* Method that draws all changes from last frame to the screen */
            FrameNum++;

            for (int y = 0; y < _consoleBuffer.GetLength(0); y++) {
                for (int x = 0; x < _consoleBuffer.GetLength(1); x++) {

                    if (newFrame.Buffer[y, x] != _consoleBuffer[y, x]) {
                        _consoleBuffer[y, x] = newFrame.Buffer[y, x];
                        Console.SetCursorPosition(x, y);
                        Console.Write(newFrame.Buffer[y, x]);
                    }

                }
            }

            _stopWatch.Reset();
        }

        public long DeltaTime() {
            /* Method that returns time since last frame */
            return _stopWatch.ElapsedMilliseconds;
        }


    }

    class Frame {
        public char[,] Buffer;
    }

}
