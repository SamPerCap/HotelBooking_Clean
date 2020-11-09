using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Moq;
using System;
using TechTalk.SpecFlow;

namespace HotelBooking.UnitTests.SpecFlow
{
    [Binding]
    public class BookRoomSteps
    {
        Mock<IRepository<Booking>> bk;
        Mock<IRepository<Room>> rm;
        private IBookingManager bkm;
        //IRepository<Booking> bk = new ;
        //IRepository<Room> rm;

        public BookRoomSteps()
        {
            bk = new Mock<IRepository<Booking>>();
            rm = new Mock<IRepository<Room>>();
            this.bkm = new BookingManager(bk.Object,rm.Object);
        }

        [Given(@"the startDate is (.*)/(.*)")]
        public void GivenTheStartDateIs(string p0, int p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"the endDate is (.*)/(.*)")]
        public void GivenTheEndDateIs(string p0, int p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the dates are compared")]
        public void WhenTheDatesAreCompared()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
