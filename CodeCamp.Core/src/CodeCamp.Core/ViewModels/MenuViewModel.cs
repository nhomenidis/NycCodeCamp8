﻿using System.Collections.Generic;
using System.Windows.Input;
using CodeCamp.Core.ViewModels.Annotations;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace CodeCamp.Core.ViewModels
{
    [DoesNotRequireLoading]
    public class MenuViewModel : ViewModelBase
    {
        public MenuViewModel(IMvxMessenger messenger) 
            : base(messenger)
        {
        }

        public ICommand ShowOverviewCommand
        {
            get
            {
                return new MvxCommand(clearStackAndShow<OverviewViewModel>);
            }
        }

        public ICommand ShowSessionsCommand
        {
            get
            {
                return new MvxCommand(clearStackAndShow<SessionsViewModel>);
            }
        }

        public ICommand ShowSpeakersCommand
        {
            get
            {
                return new MvxCommand(clearStackAndShow<SpeakersViewModel>);
            }
        }

        public ICommand ShowMapCommand
        {
            get
            {
                return new MvxCommand(clearStackAndShow<MapViewModel>);
            }
        }

        public ICommand ShowSponsorsCommand
        {
            get
            {
                return new MvxCommand(clearStackAndShow<SponsorsViewModel>);
            }
        }

        private void clearStackAndShow<TViewModel>()
            where TViewModel : ViewModelBase
        {
            var presentationBundle = new MvxBundle(new Dictionary<string, string> { { PresentationBundleFlagKeys.ClearStack, "" } });

            ShowViewModel<TViewModel>(presentationBundle: presentationBundle);
        }
    }
}