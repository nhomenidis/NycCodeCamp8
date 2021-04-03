﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeCamp.Core.Data.Entities;
using CodeCamp.Core.Messaging.Messages;
using CodeCamp.Core.ViewModels;
using MvvmCross.Core.Platform;
using NUnit.Framework;

namespace CodeCamp.Core.Tests.ViewModelTests
{
    public class SponsorsViewModelTests : ViewModelTestsBase
    {
        [Test]
        public async void Init_DataLoadsSuccessfully_LoadsSponsorList()
        {
            var sponsor = new Sponsor {Name = "Sponsor", Tier = "Gold"};
            var data = new CampData { Sponsors = new List<Sponsor> { sponsor } };
            DataClient.GetDataBody = () => Task.FromResult(data);
            var viewModel = new SponsorsViewModel(Messenger, CodeCampService);

            Assert.True(viewModel.IsLoading);

            await viewModel.Init();

            //Assert.AreEqual(data.Sponsors, viewModel.Sponsors);
            Assert.AreEqual(1, viewModel.SponsorTiers.Count);
            Assert.AreEqual(1, viewModel.SponsorTiers.First().Sponsors.Count);
            Assert.AreEqual(sponsor, viewModel.SponsorTiers.First().Sponsors.First());
            Assert.False(viewModel.IsLoading);
        }

        [Test]
        public async void Init_ExceptionThrown_ReportsError()
        {
            DataClient.GetDataBody = delegate { throw new Exception(); };
            string errorMessage = null;
            Messenger.Subscribe<ErrorMessage>(msg => errorMessage = msg.Message);

            var viewModel = new SponsorsViewModel(Messenger, CodeCampService);
            await viewModel.Init();

            Assert.NotNull(errorMessage);
            Assert.False(viewModel.IsLoading);
            Assert.Null(viewModel.SponsorTiers);
        }

        [Test]
        public void ViewSessionCommand_NavigatesToSession()
        {
            var sponsor = new Sponsor { Id = 42 };
            var viewModel = new SponsorsViewModel(Messenger, CodeCampService);

            viewModel.ViewSponsorCommand.Execute(sponsor);

            Assert.AreEqual(1, Dispatcher.ShowViewModelRequests.Count);

            var request = Dispatcher.ShowViewModelRequests.Single();
            var navParameters = request.ParameterValues.Read(typeof(SponsorViewModel.NavigationParameters)) as SponsorViewModel.NavigationParameters;
            Assert.NotNull(navParameters);
            Assert.AreEqual(sponsor.Id, navParameters.Id);
        }
    }
}