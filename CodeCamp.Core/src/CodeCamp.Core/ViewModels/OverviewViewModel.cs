using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class OverviewViewModel : ViewModelBase
    {
        private readonly ICodeCampService _campService;

        public OverviewViewModel(IMvxMessenger messenger, ICodeCampService campService)
            : base(messenger)
        {
            _campService = campService;
        }

        private IList<TimeSlot> _timeSlots;
        public IList<TimeSlot> TimeSlots
        {
            get { return _timeSlots; }
            set { _timeSlots = value; RaisePropertyChanged(() => TimeSlots); }
        } 

        public async TaskEx Init()
        {
            bool successful = await SafeOperation(
                TaskEx.Run(async () => TimeSlots = await getNextTwoSlotsAsync()));

            FinishedLoading(successful);
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
                        TimeSlots = await getNextTwoSlotsAsync();
                    }), () => IsRefreshing);

                    if (successful && DataRefreshComplete != null)
                        DataRefreshComplete.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand ViewSessionCommand
        {
            get
            {
                return new MvxCommand<Session>(
                    session => ShowViewModel<SessionViewModel>(new SessionViewModel.NavigationParameters(session.Id)));
            }
        }

        public ICommand ViewFullScheduleCommand
        {
            get { return new MvxCommand(() => ShowViewModel<SessionsViewModel>()); }
        }

        private async Task<IList<TimeSlot>> getNextTwoSlotsAsync()
        {
            return (from slot in await _campService.ListSessions()
                    where slot.EndTime >= DateTime.UtcNow
                    orderby slot.StartTime
                    select slot).Take(2).ToList();
        } 
    }
}