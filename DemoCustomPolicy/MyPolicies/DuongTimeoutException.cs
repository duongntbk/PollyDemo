using System;

namespace DemoCustomPolicy.MyPolicies
{
    public class DuongTimeoutException : Exception
    {
        public DuongTimeoutException(string message) :
            base(message)
        {
        }
    }
}
