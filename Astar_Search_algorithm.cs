using System;

using System.Collections.Generic;

using System.Drawing;

using System.Windows.Forms;



namespace FindParth

{

    public partial class Form1 : Form

    {

        private int scot, shang;

        private Label[,] mapShow;

        private bool[,] vtMap;

        private bool havePath = false;

        private bool[,] Daxet;

        private Node[,] nodes;

        private Timer timerBFS, timerAstar;

        private int topStart=0, leftStart=0, topDest=0, leftDest=0;

        private Position[] nextNode = {new Position(-1, 0), new Position(-1, 1),

            new Position(0, 1), new Position(1, 1), new Position(1, 0), new Position(1, -1),

            new Position(0, -1), new Position(-1, -1)};



        public Form1()

        {

            scot = 28; shang = 18;

            nodes = new Node[shang + 2, scot + 2];

            mapShow = new Label[shang + 2, scot + 2];

            vtMap = new bool[shang + 2, scot + 2];

            Daxet = new bool[shang + 2, scot + 2];



            InitializeComponent();

            rdBtnWall.Checked = true;

            rdBtnAstar.Checked = true;

            rdbtnStep.Checked = true;



            InitMap();

            InitTimer();

        }



        private void InitMap()

        {

            for(int i = 1; i <= shang; i++)

                for(int j = 1; j<=scot; j++)

                {

                    mapShow[i, j] = new Label();

                    mapShow[i, j].Parent = this;

                    mapShow[i, j].Top = i*21;

                    mapShow[i, j].Left = j*21;

                    mapShow[i, j].Size = new Size(20, 20);

                    mapShow[i, j].BackColor = Color.Snow;

                    mapShow[i, j].Click += Lb_Click;



                    vtMap[i, j] = true;

                    nodes[i, j] = new Node();

                }

        }



        private void InitTimer()

        {

            timerBFS = new Timer();

            timerAstar = new Timer();

            timerBFS.Interval = 50;

            timerAstar.Interval = 50;



            timerBFS.Tick += TimerBFS_Tick;

            timerAstar.Tick += TimerAstar_Tick;

        }



        private void ShowPath(Position position)

        {

            position = nodes[position.top, position.left].parent;

            while (position.top != topStart || position.left != leftStart)

            {

                mapShow[position.top, position.left].BackColor = Color.Blue;

                position = nodes[position.top, position.left].parent;

            }

        }





        

        Position currentPosition;

        int tmpTop;

        int tmpLeft;





        //========================== BFS Algorithm  =================================//



        Queue<Position> queue;

        private void BFS()

        {

            queue = new Queue<Position>();

            queue.Enqueue(new Position(topStart, leftStart));

            Daxet[topStart, leftStart] = true;

            

            while (queue.Count != 0)

            {

                currentPosition = queue.Dequeue();

                if (currentPosition.top == topDest && currentPosition.left == leftDest)

                {

                    ShowPath(currentPosition);

                    havePath = true;

                    break;

                }



                for (int i = 0; i < 8; i++)

                {

                    tmpTop = currentPosition.top + nextNode[i].top;

                    tmpLeft = currentPosition.left + nextNode[i].left;



                    if (tmpTop > 0 && tmpTop <= shang && tmpLeft > 0 && tmpLeft <= scot

                        && vtMap[tmpTop, tmpLeft] && !Daxet[tmpTop, tmpLeft])

                    {

                        nodes[tmpTop, tmpLeft].parent.top = currentPosition.top;

                        nodes[tmpTop, tmpLeft].parent.left = currentPosition.left;

                        queue.Enqueue(new Position(tmpTop, tmpLeft));

                        Daxet[tmpTop, tmpLeft] = true;

                    }

                }

            }

        }





        private void BFS_Step()

        {

            queue = new Queue<Position>();

            queue.Enqueue(new Position(topStart, leftStart));



            Daxet[topStart, leftStart] = true;

            timerBFS.Start();



        }





        private void TimerBFS_Tick(object sender, EventArgs e)

        {

            if (queue.Count != 0)

            {

                currentPosition = queue.Dequeue();

                Daxet[topStart, leftStart] = true;



                if (currentPosition.top == topDest && currentPosition.left == leftDest)

                {

                    ShowPath(currentPosition);

                    timerBFS.Stop();

                }



                if (!(currentPosition.top == topStart && currentPosition.left == leftStart

                    || currentPosition.top == topDest && currentPosition.left == leftDest))

                    mapShow[currentPosition.top, currentPosition.left].BackColor = Color.LightSkyBlue;



                for (int i = 0; i < 8; i++)

                {

                    tmpTop = currentPosition.top + nextNode[i].top;

                    tmpLeft = currentPosition.left + nextNode[i].left;



                    if (tmpTop > 0 && tmpTop <= shang && tmpLeft > 0 && tmpLeft <= scot

                        && vtMap[tmpTop, tmpLeft] && !Daxet[tmpTop, tmpLeft])

                    {

                        nodes[tmpTop, tmpLeft].parent.top = currentPosition.top;

                        nodes[tmpTop, tmpLeft].parent.left = currentPosition.left;

                        queue.Enqueue(new Position(tmpTop, tmpLeft));

                        Daxet[tmpTop, tmpLeft] = true;

                    }

                }

            }

            else

            {

                timerBFS.Stop();

                MessageBox.Show("Không tìm được đường đi đến đích");

            }

        }



        //==========================================================================//





        //========================== AStar Algorithm =================================//





        double tmpG, tmpF;

        double[] dis = { 1, 1.4, 1, 1.4, 1, 1.4, 1, 1.4 };

        LinkedList<Position> listWait;



        private Position Get_Min()

        {

            var positionMin = listWait.First.Value;

            

            foreach(Position tmp in listWait)

            {

                if(nodes[tmp.top, tmp.left].f < nodes[positionMin.top, positionMin.left].f)

                {

                    positionMin = tmp;

                }

            }

            return positionMin;

        }



        private bool IsInQueue(Position position)

        {

            return listWait.Contains(position);

        }



        private double H(Position position)

        {



            return Math.Sqrt((position.top - topDest) * (position.top - topDest)

                + (position.left - leftDest) * (position.left - leftDest));

        }

    



        private void AStar()

        {

            listWait = new LinkedList<Position>();

            listWait.AddLast(new Position(topStart, leftStart));

    

            while (listWait.Count != 0)

            {

                currentPosition = Get_Min();

                listWait.Remove(currentPosition);

                Daxet[currentPosition.top, currentPosition.left] = true;



                if (currentPosition.top == topDest && currentPosition.left == leftDest)

                {

                    ShowPath(currentPosition);

                    havePath = true;

                    break;

                }



                for (int i = 0; i < 8; i++)

                {

                    tmpTop = currentPosition.top + nextNode[i].top;

                    tmpLeft = currentPosition.left + nextNode[i].left;



                    if (tmpTop > 0 && tmpTop <= shang && tmpLeft > 0 && tmpLeft <= scot

                        && vtMap[tmpTop, tmpLeft] && !Daxet[tmpTop, tmpLeft])

                    {

                        tmpG = nodes[currentPosition.top, currentPosition.left].nodes + dis[i];

                        tmpF = tmpG + H(new Position(tmpTop, tmpLeft));



                        if (!IsInQueue(new Position(tmpTop, tmpLeft)))

                        {

                            nodes[tmpTop, tmpLeft].parent.top = currentPosition.top;

                            nodes[tmpTop, tmpLeft].parent.left = currentPosition.left;

                            nodes[tmpTop, tmpLeft].nodes = tmpG;

                            nodes[tmpTop, tmpLeft].f = tmpF;

                            listWait.AddLast(new Position(tmpTop, tmpLeft));

                        }

                        else if(nodes[tmpTop, tmpLeft].f > tmpF)

                        {

                            nodes[tmpTop, tmpLeft].parent.top = currentPosition.top;

                            nodes[tmpTop, tmpLeft].parent.left = currentPosition.left;

                            nodes[tmpTop, tmpLeft].nodes = tmpG;

                            nodes[tmpTop, tmpLeft].f = tmpF;

                        }

                        

                    }

                }

            }

        }





        private void AStar_Step()

        {

            listWait = new LinkedList<Position>();

            listWait.AddLast(new Position(topStart, leftStart));



            timerAstar.Start();

        }



        private void TimerAstar_Tick(object sender, EventArgs e)

        {

            if (listWait.Count != 0)

            {

                currentPosition = Get_Min();

                listWait.Remove(currentPosition);

                Daxet[currentPosition.top, currentPosition.left] = true;



                if (currentPosition.top == topDest && currentPosition.left == leftDest)

                {

                    ShowPath(currentPosition);

                    havePath = true;

                    timerAstar.Stop();

                }



                if (!(currentPosition.top == topStart && currentPosition.left == leftStart

                        || currentPosition.top == topDest && currentPosition.left == leftDest))

                {

                    mapShow[currentPosition.top, currentPosition.left].BackColor = Color.LightSkyBlue;

                }

                    



                for (int i = 0; i < 8; i++)

                {

                    tmpTop = currentPosition.top + nextNode[i].top;

                    tmpLeft = currentPosition.left + nextNode[i].left;



                    if (tmpTop > 0 && tmpTop <= shang && tmpLeft > 0 && tmpLeft <= scot

                        && vtMap[tmpTop, tmpLeft] && !Daxet[tmpTop, tmpLeft])

                    {

                        tmpG = nodes[currentPosition.top, currentPosition.left].nodes + dis[i];

                        tmpF = tmpG + H(new Position(tmpTop, tmpLeft));



                        if (!IsInQueue(new Position(tmpTop, tmpLeft)))

                        {

                            nodes[tmpTop, tmpLeft].parent.top = currentPosition.top;

                            nodes[tmpTop, tmpLeft].parent.left = currentPosition.left;

                            nodes[tmpTop, tmpLeft].nodes = tmpG;

                            nodes[tmpTop, tmpLeft].f = tmpF;

                            listWait.AddLast(new Position(tmpTop, tmpLeft));

                        }

                        else if (nodes[tmpTop, tmpLeft].f > tmpF)

                        {

                            nodes[tmpTop, tmpLeft].parent.top = currentPosition.top;

                            nodes[tmpTop, tmpLeft].parent.left = currentPosition.left;

                            nodes[tmpTop, tmpLeft].nodes = tmpG;

                            nodes[tmpTop, tmpLeft].f = tmpF;

                        }



                    }

                }

            }

            else

            {

                timerAstar.Stop();

                MessageBox.Show("Không tìm được đường đi đến đích");

            }

                

        }



        //==========================================================================//







        private void Lb_Click(object sender, EventArgs e)

        {

            Label lbb = (Label)sender;

            int top = lbb.Top / 21, left = lbb.Left / 21;

            if (rdBtnWall.Checked)

            {

                lbb.BackColor = Color.Black;

                vtMap[top, left] = false;

            }



            else if (rdBtnStart.Checked)

            {

                if (topStart != 0 && leftStart != 0)

                    mapShow[topStart, leftStart].BackColor = Color.Snow;



                lbb.BackColor = Color.Yellow;

                topStart = top; leftStart = left;

            }



            else if (rdBtnDest.Checked)

            {

                if (topDest != 0 && leftDest != 0)

                    mapShow[topDest, leftDest].BackColor = Color.Snow;



                lbb.BackColor = Color.Red;

                topDest = top; leftDest = left;

            }



            else

            {

                lbb.BackColor = Color.Snow;

                vtMap[top, left] = true;

            }

        }





        private void btnNew_Click(object sender, EventArgs e)

        {

            for (int i = 1; i <= shang; i++)

                for (int j = 1; j <= scot; j++)

                {

                    mapShow[i, j].BackColor = Color.Snow;

                    vtMap[i, j] = true;

                    Daxet[i, j] = false;

                }

        }



        private void btnFind_Click(object sender, EventArgs e)

        {

            if(rdBtnOnlyResult.Checked)

            {

                if (rdBtnBFS.Checked)

                {

                    BFS();

                }

                else

                {

                    AStar();

                }

                if (!havePath)

                {

                    MessageBox.Show("Không tìm được đường đi đến đích");

                }

            }

            else

            {

                if (rdBtnBFS.Checked)

                {

                    BFS_Step();

                }

                else

                {

                    AStar_Step();

                }

                

            }

        }



        

    }



    struct Position

    {

        public int top;

        public int left;

        public Position(int top, int left)

        {

            this.top = top;

            this.left = left;

        }

    }



    class Node

    {

        public Position parent;

        public double f;

        public double nodes;

        public Node()

        {

            nodes = 0;

            f = 0;

            parent.top = 0;

            parent.left = 0;

        }

    }

}