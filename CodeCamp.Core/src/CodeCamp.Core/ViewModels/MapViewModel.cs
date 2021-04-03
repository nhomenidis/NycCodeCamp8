using CodeCamp.Core.ViewModels.Annotations;
using MvvmCross.Plugins.Messenger;

namespace CodeCamp.Core.ViewModels
{
    [DoesNotRequireLoading]
    public class MapViewModel : ViewModelBase
    {
        public MapViewModel(IMvxMessenger messenger) 
            : base(messenger)
        {
        }
    }
}