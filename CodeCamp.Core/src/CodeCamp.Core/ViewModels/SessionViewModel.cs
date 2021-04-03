﻿using System.Windows.Input;
using CodeCamp.Core.Data.Entities;
using CodeCamp.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
#if !WINDOWS_PHONE
using TaskEx = System.Threading.Tasks.Task;
#endif

namespace CodeCamp.Core.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICodeCampService _campService;

        public SessionViewModel(IMvxMessenger messenger, ICodeCampService campService) 
            : base(messenger)
        {
            _campService = campService;
        }

        private Session _session;
        public Session Session
        {
            get { return _session; }
            set { _session = value; RaisePropertyChanged(() => Session); }
        }

        public async TaskEx Init(NavigationParameters parameters)
        {
            bool successful = await SafeOperation(
                TaskEx.Run(async () => Session = await _campService.GetSession(parameters.Id)));

            FinishedLoading(successful);
        }

        public ICommand ViewSpeakerCommand
        {
            get
            {
                return new MvxCommand(() => 
                {
                    if (Session.SpeakerId.HasValue)
                        ShowViewModel<SpeakerViewModel>(new SpeakerViewModel.NavigationParameters(Session.SpeakerId.Value));
                });
            }
        }

        public class NavigationParameters
        {
            public int Id { get; set; }

            public NavigationParameters()
            {
            }

            public NavigationParameters(int id)
            {
                Id = id;
            }
        }
    }
}