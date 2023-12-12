using PangeaCode.Countries;
using PangeaCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangeaCode.Partners
{
    public interface IPartner
    {
        //Each Partner will also have its own individual httpClient to handle all calls to their API
        //Probably a static or singleton HttpClient instance with PooledConnectionLifetime set to the desired interval,
        //such as 2 minutes, depending on any expected DNS changes.
        //This solves both the port exhaustion and DNS change issues
        //Or we could use the httpClientFactory, but before I can decide that I need to get more information
        //about the expected requirements of the various partner API, and also if we are only going to call
        //API methods to get rates, or if we anticipate making additional calls to the API for other reasons

        Task<List<PartnerRate>> GetPartnerRatesAsync(string countryCode); 
    }
}
