using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LogReader.HelperClasses;

namespace LogReader
{
    public class Reader
    {        
        private string path;      
       
        public Reader(string path)
        {
            this.path = path;     
        }           

        Func<string, string, bool> IsAdd = (line, filter) => line.ContainsIgnoreCase(filter);
        Func<string, string, bool> AlwaysAdd = (line, filter) => true;        

        
        public IEnumerable<string> GetFileContent(string filter = null){
            
            if (String.IsNullOrWhiteSpace(filter))
            {                
                return ReadFile(AlwaysAdd);
            }
            else
            {             
                return ReadFile(IsAdd, filter);
            }
        }
      
        private IEnumerable<string> ReadFile(Func<string, string, bool> IsAddMethod, string filter = null)
        {
            List<string> list = new List<string>();
            foreach (string line in ReadLines(path))
            {
                if (IsAddMethod(line, filter))
                    list.Add(line);
            }          

            return list;
        }     

        private static IEnumerable<string> ReadLines(string path)
        {            
            using (StreamReader sr = new StreamReader (
                    new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }       
    }
}
