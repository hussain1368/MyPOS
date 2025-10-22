using System;

namespace POS.DAL.Common
{
    public class MyException : Exception
    {
        public MyException(string message) : base(message) { }
    }
}
