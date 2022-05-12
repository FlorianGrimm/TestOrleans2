namespace Replacement.WebApp.Utility {
    public static class ExceptionExtensions
    {
        public static bool WriteError(Exception error)
        {
            Console.Error.WriteLine(error.ToString());
            if (error is AggregateException aggregateException)
            {
                aggregateException.Handle(innerWriteError);
                //foreach (var innerException in aggregateException.InnerExceptions) {
                //    writeError(innerException);
                //}
            }
            return true;
        }

        private static bool innerWriteError(Exception error)
        {
            Console.Error.WriteLine(error.ToString());
            if (error is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    innerWriteError(innerException);
                }
            }
            return true;
        }

    }
}
