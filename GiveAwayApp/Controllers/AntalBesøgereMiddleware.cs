using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GiveAwayApp.Data;
using GiveAwayApp.Models;

namespace GiveAwayApp.Controllers
{
    public class AntalBesøgereMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        public AntalBesøgereMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext httpContext, GiveAwayAppContext appContext)
        {
            var statistikQuery = from statistik in appContext.Statistik select statistik;

            if (await statistikQuery.AnyAsync(s => s.AntalBesøgereForDato == DateTime.UtcNow.Date))
            {
                Statistik opdaterStatistik = await statistikQuery.FirstAsync(s => s.AntalBesøgereForDato == DateTime.UtcNow.Date);
                opdaterStatistik.AntalBesøgere++;

                appContext.Update(opdaterStatistik);
            }
            else
            {
                Statistik NyStatistik = new()
                {
                    AntalBesøgere = 1,
                    AntalBesøgereForDato = DateTime.UtcNow.Date
                };

                appContext.Statistik.Add(NyStatistik);
            }

            await appContext.SaveChangesAsync();

            await _requestDelegate(httpContext);
        }
    }
}