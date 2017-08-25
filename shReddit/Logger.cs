namespace shReddit
{
    public class Logger
    {
        public string LogEntry(string currentText, string message, bool append)
        {

            if (append)
            {
                return (currentText + BuildMessage(message));
            }
            else
            {
                return (BuildMessage(message) + currentText);
            }
            
        }        

        private string BuildMessage(string message)
        {
            return $"\r\n{System.DateTime.Now} | " + message;
        }
    }
}
