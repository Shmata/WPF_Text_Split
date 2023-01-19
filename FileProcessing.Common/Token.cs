namespace FileProcessing.Common
{
    public class Token
    {
        public string Word { get; set; }
        public int Occurrence { get; set; }

        public Token(string word, int occurrence)
        {
            this.Word = word;
            this.Occurrence = occurrence;
        }
    }
}
