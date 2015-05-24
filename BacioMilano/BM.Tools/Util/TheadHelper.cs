using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace BM.Util
{
    public static class TheadHelper
    {
        public static void BackgroundWorkerRun(Action start, Action run, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run != null)
                    {
                        run();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }


        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }


        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }

        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action run4, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }

                else if (e1.ProgressPercentage == 5)
                {
                    if (run4 != null)
                    {
                        run4();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(5);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }

        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action run4, Action run5, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }

                else if (e1.ProgressPercentage == 5)
                {
                    if (run4 != null)
                    {
                        run4();
                    }
                }

                else if (e1.ProgressPercentage == 6)
                {
                    if (run5 != null)
                    {
                        run5();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(5);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(6);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }

        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action run4, Action run5, Action run6, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }

                else if (e1.ProgressPercentage == 5)
                {
                    if (run4 != null)
                    {
                        run4();
                    }
                }

                else if (e1.ProgressPercentage == 6)
                {
                    if (run5 != null)
                    {
                        run5();
                    }
                }
                else if (e1.ProgressPercentage == 7)
                {
                    if (run6 != null)
                    {
                        run6();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(5);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(6);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(7);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }

        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action run4, Action run5, Action run6, Action run7, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }

                else if (e1.ProgressPercentage == 5)
                {
                    if (run4 != null)
                    {
                        run4();
                    }
                }

                else if (e1.ProgressPercentage == 6)
                {
                    if (run5 != null)
                    {
                        run5();
                    }
                }
                else if (e1.ProgressPercentage == 7)
                {
                    if (run6 != null)
                    {
                        run6();
                    }
                }
                else if (e1.ProgressPercentage == 8)
                {
                    if (run7 != null)
                    {
                        run7();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(5);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(6);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(7);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(8);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }

        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action run4, Action run5, Action run6, Action run7, Action run8, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }

                else if (e1.ProgressPercentage == 5)
                {
                    if (run4 != null)
                    {
                        run4();
                    }
                }

                else if (e1.ProgressPercentage == 6)
                {
                    if (run5 != null)
                    {
                        run5();
                    }
                }
                else if (e1.ProgressPercentage == 7)
                {
                    if (run6 != null)
                    {
                        run6();
                    }
                }
                else if (e1.ProgressPercentage == 8)
                {
                    if (run7 != null)
                    {
                        run7();
                    }
                }
                else if (e1.ProgressPercentage == 9)
                {
                    if (run8 != null)
                    {
                        run8();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(5);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(6);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(7);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(8);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(9);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }

        public static void BackgroundWorkerRun(Action start, Action run1, Action run2, Action run3, Action run4, Action run5, Action run6, Action run7, Action run8, Action run9, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (e1.ProgressPercentage == 2)
                {
                    if (run1 != null)
                    {
                        run1();
                    }
                }
                else if (e1.ProgressPercentage == 3)
                {
                    if (run2 != null)
                    {
                        run2();
                    }
                }

                else if (e1.ProgressPercentage == 4)
                {
                    if (run3 != null)
                    {
                        run3();
                    }
                }

                else if (e1.ProgressPercentage == 5)
                {
                    if (run4 != null)
                    {
                        run4();
                    }
                }

                else if (e1.ProgressPercentage == 6)
                {
                    if (run5 != null)
                    {
                        run5();
                    }
                }
                else if (e1.ProgressPercentage == 7)
                {
                    if (run6 != null)
                    {
                        run6();
                    }
                }
                else if (e1.ProgressPercentage == 8)
                {
                    if (run7 != null)
                    {
                        run7();
                    }
                }
                else if (e1.ProgressPercentage == 9)
                {
                    if (run8 != null)
                    {
                        run8();
                    }
                }
                else if (e1.ProgressPercentage == 10)
                {
                    if (run9 != null)
                    {
                        run9();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(2);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(3);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(4);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(5);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(6);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(7);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(8);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(9);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(10);
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }


        public static void BackgroundWorkerRun(Action start, Dictionary<int, Action> runs, Action finish)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage == 1)
                {
                    if (start != null)
                    {
                        start();
                    }
                }
                else if (runs.ContainsKey(e1.ProgressPercentage))
                {
                    var a = runs[e1.ProgressPercentage];
                    if (a != null)
                    {
                        a();
                    }
                }
                else if (e1.ProgressPercentage == 100)
                {
                    if (finish != null)
                    {
                        finish();
                    }
                }
            };
            backgroundWorker1.DoWork += delegate
            {
                backgroundWorker1.ReportProgress(1);
                for (int i = 2; i < 100; i++)
                {
                    backgroundWorker1.ReportProgress(i);
                    Thread.Sleep(20);
                }
                backgroundWorker1.ReportProgress(100);
            };
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
