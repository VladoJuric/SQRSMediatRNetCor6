namespace Infrastructure.Logger
{
    public static class ExceptionExtensions
    {
        public static string InnermostMessage(this Exception e)
        {
            while (true)
            {
                if (e.InnerException == null) return e.Message;
                e = e.InnerException;
            }
        }
    }
}
