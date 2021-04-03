﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using CodeCamp.Core.Data.Entities;
using CodeCamp.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
#if !WINDOWS_PHONE
using TaskEx = System.Threading.Tasks.Task;
#endif

namespace CodeCamp.Core.ViewModels
{
    public class SponsorsViewModel : ViewModelBase
    {
        private readonly ICodeCampService _campService;

        public SponsorsViewModel(IMvxMessenger messenger, ICodeCampService campService) 
            : base(messenger)
        {
            _campService = campService;
        }

        private IList<SponsorTier> _sponsorTiers;
        public IList<SponsorTier> SponsorTiers
        {
            get { return _sponsorTiers; }
            set { _sponsorTiers = value; RaisePropertyChanged(() => SponsorTiers); }
        } 

        public async TaskEx Init()
        {
            bool successful = await SafeOperation(
                TaskEx.Run(async () => SponsorTiers = await _campService.ListSponsors()));

            FinishedLoading(successful);
        }

        public ICommand ViewSponsorCommand
        {
            get
            {
                return new MvxCommand<Sponsor>(
                    sponsor => ShowViewModel<SponsorViewModel>(new SponsorViewModel.NavigationParameters(sponsor.Id)));
            }
        }

        public event EventHandler DataRefreshComplete;

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value; RaisePropertyChanged(() => IsRefreshing); }
        }

        public ICommand RefreshDataCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    bool successful = await SafeOperation(TaskEx.Run(async () => 
                    {
                        await _campService.RefreshData();
                        SponsorTiers = await _campService.ListSponsors();
                    }), () => IsRefreshing);

                    if (successful && DataRefreshComplete != null)
                        DataRefreshComplete.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}