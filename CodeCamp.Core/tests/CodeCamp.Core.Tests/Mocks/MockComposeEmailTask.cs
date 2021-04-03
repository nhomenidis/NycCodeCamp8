using System.Collections.Generic;
using MvvmCross.Plugins.Email;

namespace CodeCamp.Core.Tests.Mocks
{
    public class MockComposeEmailTask : IMvxComposeEmailTask
    {
        public IList<string> ComposedEmailAddresses { get; set; }

        public MockComposeEmailTask()
        {
            ComposedEmailAddresses = new List<string>();
        }
        
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false,
            string dialogTitle = null)
        {
            ComposedEmailAddresses.Add(to);
        }
    }
}