using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Core.Exceptions
{
    //adapted from: https://stackoverflow.com/questions/5928976/what-is-the-proper-way-to-display-the-full-innerexception/52042708#52042708
    public static class ExceptionExtensions
    {
        public static List<string> GetErrorMessages(this Exception exception)
        {
            return exception
                .GetAllExceptions()
                .Where(e => !String.IsNullOrWhiteSpace(e.Message))
                .Select(e => e.Message.Trim()).ToList();
        }

        public static IEnumerable<Exception> GetAllExceptions(this Exception exception)
        {
            yield return exception;

            if (exception is AggregateException aggrEx)
            {
                foreach (Exception innerEx in aggrEx.InnerExceptions.SelectMany(e => e.GetAllExceptions()))
                {
                    yield return innerEx;
                }
            }
            else if (exception.InnerException != null)
            {
                foreach (Exception innerEx in exception.InnerException.GetAllExceptions())
                {
                    yield return innerEx;
                }
            }
        }
    }
}