using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFReader;
using System.Windows.Input;
using LogReader.HelperClasses;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;
using System.Collections.ObjectModel;


namespace LogReader
{
   

    public class MainViewModel : ObservableObject
    {
        #region fields && properties
            const string AppName = "Log Reader";    
            public string DisplayFileName { get; private set; }
            Reader reader ;      
    
            private ICommand openFileCommand; 
            private ICommand getResultsCommand;
            private string findText;

            public List<string> Results { get; private set; }
            public bool IsFindEnabled { get; private set; }
            public string StatusMessage { get; private set; }
            public string ResultString { get; set; }      

            public string FindText { 
                
                get {
                    return findText;
                } 
                
                set {
                    findText = value.Trim();
                    if (findText.Length > 0)
                    {
                        GetResultsCommand.Execute(null);
                    }
                } 
            }
         

        #endregion

            public MainViewModel()
            {
                DisplayFileName = AppName;             
            }

        #region Commands
        public ICommand GetResultsCommand
        {
            get {
                if (getResultsCommand == null)
                {
                    getResultsCommand = new RelayCommand(param => GetResults());
                }
                return getResultsCommand;
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                if (openFileCommand == null)
                {
                    openFileCommand = new RelayCommand(param => OpenFile());
                }
                return openFileCommand;
            }
        }     
        #endregion

        private string getTitle(string path)
        {
           return String.Concat(AppName, " - " + path); 
        }

        private void OpenFile()
        {
            var dialog = new OpenFileDialog ();
            dialog.Filter = "Text files (*.txt, *.log)|*.txt;*.log";

            DialogResult result = dialog.ShowDialog();   

            if (result == DialogResult.OK)
            {
                string path = dialog.FileName;
                DisplayFileName = getTitle (Path.GetFileName(path));                
                try
                {
                    ReadFile(path);                 
                    IsFindEnabled = true;
                    OnPropertyChanged("DisplayFileName");
                    OnPropertyChanged("IsFindEnabled");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void ReadFile(string path)
        {
            reader = new Reader(path);

            ThreadPool.QueueUserWorkItem(new WaitCallback(
                    si => GetResults()
                ));
            
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();  
            dispatcherTimer.Tick += new EventHandler(
                    (s, e) => GetResults()
                );

            //refresh every 30 seconds
            dispatcherTimer.Interval = new TimeSpan (0, 0,30);
            dispatcherTimer.Start();
        }          
                

        private void GetResults()
        {
            try
            {                
                Stopwatch sw = Stopwatch.StartNew();                
               
                Results = reader.GetFileContent(FindText).ToList();

                ResultString = Results.ListToString();
                OnPropertyChanged("ResultString");
                
                StatusMessage = String.Format("Found {0} results; time taken: {1} ms; last updated at {2}", Results.Count(), sw.Elapsed.Milliseconds, DateTime.Now);                
                sw.Stop();
                OnPropertyChanged("StatusMessage");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Log Reader");
            }
        }             

    }
}
