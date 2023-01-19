using FileProcessing.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessing.Business
{
    public class WordProcessor
    {
        public bool LoadCollectionData(IEnumerable<string> content, CancellationTokenSource canceller, Dictionary<string, Token> tokens)
        {

            foreach (string contentPart in content)
            {
                foreach (string word in contentPart.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Split(' '))
                {
                    if (canceller.IsCancellationRequested)
                    {
                        return false;
                    }
                    string trimed = word.Trim();
                    if (!string.IsNullOrWhiteSpace(trimed))
                    {
                        if (tokens.ContainsKey(trimed))
                        {
                            tokens[trimed].Occurrence++;
                        }
                        else
                        {
                            tokens.Add(trimed, new Token(trimed, 1));
                        }
                    }
                }
            }
            return false;
        }
    }
}

