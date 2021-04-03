using CodeCamp.Core.Services;
using CodeCamp.Core.Tests.Mocks;
using MvvmCross.Core.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Json;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Test.Core;
using NUnit.Framework;

namespace CodeCamp.Core.Tests.ViewModelTests
{
    [TestFixture]
    public abstract class ViewModelTestsBase : MvxIoCSupportingTest
    {
        protected MockMvxViewDispatcher Dispatcher { get; private set; }
        protected MockCampDataClient DataClient { get; private set; }
        protected MockComposeEmailTask ComposeEmailTask { get; private set; }
        protected MockWebBrowserTask WebBrowserTask { get; private set; }
        protected IMvxMessenger Messenger { get; private set; }
        protected ICodeCampService CodeCampService { get; private set; }

        [SetUp]
        public void SetUp()
        {
            MvxSingleton.ClearAllSingletons();
            ClearAll();

            Dispatcher = new MockMvxViewDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(Dispatcher);
            Ioc.RegisterSingleton<IMvxViewDispatcher>(Dispatcher);

            DataClient = new MockCampDataClient();
            Messenger = new MvxMessengerHub();
            CodeCampService = new CodeCampService(new InMemoryFileManager(), new MvxJsonConverter(), DataClient);
            ComposeEmailTask = new MockComposeEmailTask();
            WebBrowserTask = new MockWebBrowserTask();

            Mvx.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            SetUpFixture();
        }

        protected virtual void SetUpFixture() { }
    }
}
