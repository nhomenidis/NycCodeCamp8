﻿using System.Net.Http;
using System.Threading.Tasks;
using CodeCamp.Core.Data.Entities;
using MvvmCross.Platform.Platform;

namespace CodeCamp.Core.Network
{
    public interface ICampDataClient
    {
        Task<CampData> GetData();
    }

    public class CampDataClient : ICampDataClient
    {
        private const string DataUrl = "http://nyccodecamp8.azure-mobile.net/api/alldata";
        private readonly IMvxJsonConverter _jsonConverter;

        public CampDataClient(IMvxJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        public async Task<CampData> GetData()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(DataUrl);

                return _jsonConverter.DeserializeObject<CampData>(json);
            }
        }
    }
}
