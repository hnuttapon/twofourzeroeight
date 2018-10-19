using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace twozerofoureight
{
    class TwoZeroFourEightModel : Model
    {
        protected int boardSize; // default is 4
        protected int[,] board;
        protected Random rand;
        protected int[] range;

        public TwoZeroFourEightModel() : this(4)
        {
            // default board size is 4 
        }

        public TwoZeroFourEightModel(int size)
        {
            boardSize = size;
            board = new int[boardSize, boardSize];
            range = Enumerable.Range(0, boardSize).ToArray();
            foreach (int i in range)
            {
                foreach (int j in range)
                {
                    board[i, j] = 0;
                }
            }
            rand = new Random();
            // initialize board
            HandleChanges();
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public bool CheckGameOver() //Check if there is a box that has the value 2048
        {
            var range = Enumerable.Range(0, boardSize);

            bool check = false;
            foreach (int i in range)
            {
                foreach (int j in range)
                {
                    if (board[i, j] == 2048)
                    {
                        check = true;
                    }
                }
            }
            return check;
        }

        public bool CheckGameOverFull() //Check if all boxes contain number and whether it can continues or not
        {
            int count = 0;


            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] != 0)
                    {
                        count++;
                    }
                }
            }

            if (count == 16) //if count = 16 ; means that all boxes are full
            {
                //loop that check number in every box
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        //First row
                        if (i == 0 && j == 0) //#column 1
                        {
                            if (board[i, j] == board[i, j + 1] || board[i, j] == board[i + 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        else if (i == 0 && j != 0 && j != boardSize - 1) //#column 2,3
                        {
                            if (board[i, j] == board[i, j - 1] || board[i, j] == board[i, j + 1] || board[i, j] == board[i + 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        else if (i == 0 && j == boardSize - 1) //#column 4
                        {
                            if (board[i, j] == board[i, j - 1] || board[i, j] == board[i + 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        //Second Row and Third Row
                        else if (j == 0 && i != 0 && i != boardSize - 1) //#column 1
                        {
                            if (board[i, j] == board[i + 1, j] || board[i, j] == board[i, j + 1] || board[i, j] == board[i - 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        //Second Row and Third Row
                        else if (j == boardSize - 1 && i != 0 && i != boardSize - 1) //#column 4
                        {
                            if (board[i, j] == board[i + 1, j] || board[i, j] == board[i, j - 1] || board[i, j] == board[i - 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        //Forth Row
                        else if (i == boardSize - 1 && j == 0) //#column 1
                        {
                            if (board[i, j] == board[i, j + 1] || board[i, j] == board[i - 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        else if (i == boardSize - 1 && j != 0 && j != boardSize - 1) //#column 2,3
                        {
                            if (board[i, j] == board[i, j - 1] || board[i, j] == board[i, j + 1] || board[i, j] == board[i - 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        else if (i == boardSize - 1 && j == boardSize - 1) //#column 1
                        {
                            if (board[i, j] == board[i, j - 1] || board[i, j] == board[i - 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                        else//#box in the middle 
                        {
                            if (board[i, j] == board[i, j - 1] || board[i, j] == board[i, j + 1] || board[i, j] == board[i + 1, j] || board[i, j] == board[i - 1, j])
                            {
                                return false; //condition if the game is not over yet(board full)
                            }
                        }
                    }
                }

                return true; //condition if the game is over

            }
            return false; //condition if the game is not over yet(board not full)
        }

        public int GetScore() //Return the sum of the score
        {
            var range = Enumerable.Range(0, boardSize);
            int sum = 0;
            foreach (int i in range)
            {
                foreach (int j in range)
                {
                    sum = sum + board[i, j];
                }
            }
            return sum;
        }

        private void AddRandomSlot()
        {
            while (true)
            {
                int x = rand.Next(boardSize);
                int y = rand.Next(boardSize);
                if (board[x, y] == 0)
                {
                    board[x, y] = 2;
                    return;
                }
            }
        }

        // Perform shift and merge to the left of the given array.
        protected bool ShiftAndMerge(int[] buffer)
        {
            bool changed = false; // whether the array has changed
            int pos = 0; // next available slot index
            int lastMergedSlot = -1; // last slot that resulted from merging
            foreach (int k in range)
            {
                if (buffer[k] != 0) // nonempty slot
                {
                    // check if we can merge with the previous slot
                    if (pos > 0 && pos - 1 > lastMergedSlot && buffer[pos - 1] == buffer[k])
                    {
                        // merge
                        buffer[pos - 1] *= 2;
                        buffer[k] = 0;
                        lastMergedSlot = pos - 1;
                        changed = true;
                    }
                    else
                    {
                        // shift to the next available slot
                        buffer[pos] = buffer[k];
                        if (pos != k)
                        {
                            buffer[k] = 0;
                            changed = true;
                        }
                        // move the next available slot
                        pos++;
                    }
                }
            }
            return changed;
        }

        protected void HandleChanges(bool changed = true)
        {
            // if the board has changed, add a new number
            // and notify all views
            if (changed)
            {
                AddRandomSlot();
                NotifyAll();
            }
        }


        public void PerformDown()
        {
            if (CheckGameOver() == false)
            {

                bool changed = false; // whether the board has changed
                foreach (int i in range)
                {
                    int[] buffer = new int[boardSize];
                    // extract the current column from bottom to top
                    foreach (int j in range)
                    {
                        buffer[boardSize - j - 1] = board[j, i];
                    }
                    // process the extracted array
                    // also track changes
                    changed = ShiftAndMerge(buffer) || changed;
                    // copy back
                    foreach (int j in range)
                    {
                        board[j, i] = buffer[boardSize - j - 1];
                    }
                }
                HandleChanges(changed);
            }
        }

        public void PerformUp()
        {
            if (CheckGameOver() == false)
            {
                bool changed = false; // whether the board has changed
                foreach (int i in range)
                {
                    int[] buffer = new int[boardSize];
                    // extract the current column from top to bottom
                    foreach (int j in range)
                    {
                        buffer[j] = board[j, i];
                    }
                    // process the extracted array
                    // also track changes
                    changed = ShiftAndMerge(buffer) || changed;
                    // copy back
                    foreach (int j in range)
                    {
                        board[j, i] = buffer[j];
                    }
                }
                HandleChanges(changed);
            }
        }

        public void PerformRight()
        {
            if (CheckGameOver() == false)
            {
                bool changed = false; // whether the board has changed
                foreach (int i in range)
                {
                    int[] buffer = new int[boardSize];
                    // extract the current column from right to left
                    foreach (int j in range)
                    {
                        buffer[boardSize - j - 1] = board[i, j];
                    }
                    // process the extracted array
                    // also track changes
                    changed = ShiftAndMerge(buffer) || changed;
                    // copy back
                    foreach (int j in range)
                    {
                        board[i, j] = buffer[boardSize - j - 1];
                    }
                }
                HandleChanges(changed);
            }
        }

        public void PerformLeft()
        {
            if (CheckGameOver() == false)
            {
                bool changed = false; // whether the board has changed
                foreach (int i in range)
                {
                    int[] buffer = new int[boardSize];
                    // extract the current column from left to right
                    foreach (int j in range)
                    {
                        buffer[j] = board[i, j];
                    }
                    // process the extracted array
                    // also track changes
                    changed = ShiftAndMerge(buffer) || changed;
                    // copy back
                    foreach (int j in range)
                    {
                        board[i, j] = buffer[j];
                    }
                }
                HandleChanges(changed);
            }
        }
    }
}
