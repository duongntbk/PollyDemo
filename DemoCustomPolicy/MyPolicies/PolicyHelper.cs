using System;
using System.Collections.Generic;
using System.Text;
using Polly;

namespace DemoCustomPolicy
{
    public static class PolicyHelper
    {
        public static IAsyncPolicy RetryOnOutOfRange()
        {
            throw new NotImplementedException();
        }
    }
}
