using HotelBooking.Interfaces;
using HotelBooking.Services.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotelBooking.Test.Services.Configuration
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void DependencyInjectionClassReturnsInstanceOfBusinessLayer()
        {
            var businessLayer = Startup.Configure();

            Assert.IsNotNull(businessLayer);
            Assert.IsInstanceOfType(businessLayer, typeof(IReservationManagement));
        }
    }
}
