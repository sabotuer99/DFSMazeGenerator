using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MazeTutorial
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public void AddActivatedHandler()
        {
            this.Shown += ((e, s) => FillWithLabels());
        }
            

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void FillWithLabels()
        {
            // cellx and celly are the working coordinates, max_x and max_y are the
            // bounds of the grid
            int cellx = 1, celly = 1,
                max_x = (panel1.Width / 20), max_y = (panel1.Height / 20);

            CellType[,] grid = new CellType[max_x, max_y];

            List<Direction> validMove = new List<Direction>();

            Direction previousMove = Direction.Empty;
            Direction backTrack = Direction.Empty;

            Random randomizer = new Random();
            bool done = false;

            //fill panel with labels. 
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    Label tempLabel = new Label();
                    tempLabel.Parent = panel1;

                    int x = i * 20;
                    int y = j * 20;
                    Point loc = new Point(x, y);
                    tempLabel.AutoSize = false;
                    tempLabel.Size = new Size(20, 20);
                    tempLabel.Location = loc;
                    tempLabel.BackColor = Color.Blue;
                    tempLabel.Name = (i.ToString() + "_" + j.ToString());
                    //tempLabel.Text = tempLabel.Name;
                }
            }
            panel1.Refresh();

            //start creating maze, create starting tile and change label color
            grid[cellx, celly] = CellType.NewPass;
            panel1.Controls.Find((cellx).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;
            panel1.Refresh();

            while (!done)
            {
                panel1.Refresh();

                // mark the surrounding walls as final
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        // make sure target cell is not out of bounds (thus null)
                        if (cellx + i >= 0 && cellx + i <= max_x - 1 && celly + j >= 0
                             && celly + j <= max_y - 1)
                        {
                            //if wall is new, make it final
                            if (grid[cellx + i, celly + j] == CellType.NewWall)
                            {
                                grid[cellx + i, celly + j] = CellType.FinalWall;
                                //change corresponding label color
                                panel1.Controls.Find((cellx + i).ToString() + "_" + (celly + j).ToString(), true)[0].BackColor = Color.Black;
                            }
                        }
                    }
                }

                // look for valid moves. Target Direction should be in-bounds, 
                // not the Direction we just came from, and should be 
                // unvisited (NewWall type)
                if (cellx - 2 >= 0)
                {
                    if ((previousMove != Direction.East || previousMove == Direction.Empty)
                          && grid[cellx - 2, celly] == CellType.NewWall)
                        validMove.Add(Direction.West);
                }

                if (cellx + 2 <= max_x - 1)
                {
                    if ((previousMove != Direction.West || previousMove == Direction.Empty)
                          && grid[cellx + 2, celly] == CellType.NewWall)
                        validMove.Add(Direction.East);
                }

                if (celly + 2 <= max_y - 1)
                {
                    if ((previousMove != Direction.North || previousMove == Direction.Empty)
                          && grid[cellx, celly + 2] == CellType.NewWall)
                        validMove.Add(Direction.South);
                }

                if (celly - 2 >= 0)
                {
                    if ((previousMove != Direction.South || previousMove == Direction.Empty)
                          && grid[cellx, celly - 2] == CellType.NewWall)
                        validMove.Add(Direction.North);
                }

                //if validMove > 0 == true, then we found a valid move!
                if (validMove.Count > 0)
                {
                    // randomly select a valid move from those available
                    Direction Move = validMove[randomizer.Next(0, validMove.Count)];

                    // select next cell based on chosed Direction, 
                    // knock down wall between cells
                    switch (Move)
                    {
                        case Direction.West:

                            //intermediate passage is cleared
                            grid[cellx - 1, celly] = CellType.NewPass;
                            //change corresponding label color
                            panel1.Controls.Find((cellx - 1).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;

                            cellx = cellx - 2;

                            //target cell is cleared
                            grid[cellx, celly] = CellType.NewPass;
                            //change target label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;

                            previousMove = Direction.West;
                            break;
                        case Direction.East:

                            //intermediate passage is cleared
                            grid[cellx + 1, celly] = CellType.NewPass;
                            //change corresponding label color
                            panel1.Controls.Find((cellx + 1).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;

                            cellx = cellx + 2;

                            //target cell is cleared
                            grid[cellx, celly] = CellType.NewPass;
                            //change target label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;

                            previousMove = Direction.East;
                            break;
                        case Direction.South:

                            //intermediate passage is cleared
                            grid[cellx, celly + 1] = CellType.NewPass;
                            //change corresponding label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly + 1).ToString(), true)[0].BackColor = Color.Yellow;

                            celly = celly + 2;

                            //target cell is cleared
                            grid[cellx, celly] = CellType.NewPass;
                            //change target label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;

                            previousMove = Direction.South;
                            break;
                        case Direction.North:

                            //intermediate passage is cleared
                            grid[cellx, celly - 1] = CellType.NewPass;
                            //change corresponding label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly - 1).ToString(), true)[0].BackColor = Color.Yellow;

                            celly = celly - 2;

                            //target cell is cleared
                            grid[cellx, celly] = CellType.NewPass;
                            //change target label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.Yellow;

                            previousMove = Direction.North;
                            break;
                    }

                    // reset validMove;
                    validMove.Clear();
                }

                //No valid moves found, time to backtrack
                else
                {
                    //find the Direction we came from. first check to make sure
                    // we stay in the range of the array
                    if (cellx - 1 >= 0)
                    {
                        if (grid[cellx - 1, celly] == CellType.NewPass)
                            backTrack = Direction.West;
                    }

                    if (cellx + 1 <= max_x - 1)
                    {
                        if (grid[cellx + 1, celly] == CellType.NewPass)
                            backTrack = Direction.East;
                    }

                    if (celly + 1 <= max_y - 1)
                    {
                        if (grid[cellx, celly + 1] == CellType.NewPass)
                            backTrack = Direction.South;
                    }

                    if (celly - 1 >= 0)
                    {
                        if (grid[cellx, celly - 1] == CellType.NewPass)
                            backTrack = Direction.North;
                    }

                    //mark this cell final
                    grid[cellx, celly] = CellType.FinalPass;
                    //change label color
                    panel1.Controls.Find((cellx).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.White;

                    // move in backtrack location, mark intermediate passage as final
                    switch (backTrack)
                    {
                        case Direction.West:
                            grid[cellx - 1, celly] = CellType.FinalPass;
                            //change label color
                            panel1.Controls.Find((cellx - 1).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.White;
                            cellx = cellx - 2;
                            break;

                        case Direction.East:
                            grid[cellx + 1, celly] = CellType.FinalPass;
                            //change label color
                            panel1.Controls.Find((cellx + 1).ToString() + "_" + (celly).ToString(), true)[0].BackColor = Color.White;
                            cellx = cellx + 2;
                            break;

                        case Direction.South:
                            grid[cellx, celly + 1] = CellType.FinalPass;
                            //change label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly + 1).ToString(), true)[0].BackColor = Color.White;
                            celly = celly + 2;
                            break;

                        case Direction.North:
                            grid[cellx, celly - 1] = CellType.FinalPass;
                            //change label color
                            panel1.Controls.Find((cellx).ToString() + "_" + (celly - 1).ToString(), true)[0].BackColor = Color.White;
                            celly = celly - 2;
                            break;

                        default:
                            //nowhere to backtrack? we're done!
                            done = true;
                            break;
                    }
                    backTrack = Direction.Empty;
                    previousMove = Direction.Empty;
                    // end of "else"
                }
                // end of "while"
            }
            //string msg = "Finished in " + count.ToString() + " moves.";
            //MessageBox.Show(msg);

            //Set start and finish areas
            grid[0, 0] = CellType.Start;
            grid[1, 0] = CellType.Start;
            grid[0, 1] = CellType.Start;
            grid[1, 1] = CellType.Start;
            panel1.Controls.Find("0_0", true)[0].BackColor = Color.Green;
            panel1.Controls.Find("1_0", true)[0].BackColor = Color.Green;
            panel1.Controls.Find("0_1", true)[0].BackColor = Color.Green;
            panel1.Controls.Find("1_1", true)[0].BackColor = Color.Green;

            grid[max_x - 1, max_y - 1] = CellType.Finish;
            grid[max_x - 2, max_y - 1] = CellType.Finish;
            grid[max_x - 1, max_y - 2] = CellType.Finish;
            grid[max_x - 2, max_y - 2] = CellType.Finish;
            panel1.Controls.Find((max_x - 2).ToString() + "_" + (max_y - 2).ToString(), true)[0].BackColor = Color.Red;

            // add event listeners to walls and finish square
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    switch (grid[i, j])
                    {
                        case CellType.FinalWall:
                            panel1.Controls.Find(i.ToString() + "_" + j.ToString(), true)[0].MouseEnter += new EventHandler(HitWall);
                            break;

                        case CellType.Finish:
                            panel1.Controls.Find(i.ToString() + "_" + j.ToString(), true)[0].MouseEnter += new EventHandler(FinishMaze);
                            break;
                    }
                }
            }
        }

        private void MoveToStart()
        {
            Point startingPoint = panel1.Location;
            startingPoint.Offset(10, 10);
            Cursor.Position = PointToScreen(startingPoint);
        }

        private void FinishMaze(object sender, EventArgs e)
        {
            // Show a congratulatory MessageBox, then close the form.
            MessageBox.Show("Congratulations!");
            //Clear labels
            panel1.Controls.Clear();
            FillWithLabels();
        }

        private void HitWall(object sender, EventArgs e)
        {
            // When the mouse pointer hits a wall or enters the panel, 
            // call the MoveToStart() method.
            MoveToStart();
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 680);
            this.panel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 704);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Dumb Maze Game";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
    }
}

