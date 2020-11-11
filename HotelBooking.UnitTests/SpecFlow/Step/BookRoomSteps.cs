using System;
using TechTalk.SpecFlow;

namespace HotelBooking.UnitTests.SpecFlow.Step
{
    [Binding]
    public class BookRoomSteps
    {
        DateTime Tomorrow = DateTime.Today.AddDays(1);
        DateTime StartOccupiedRange = DateTime.Today.AddDays(3);
        DateTime EndOccupiedRange = DateTime.Today.AddDays(6);

        [Given(@"the startDate which is tomorrow plus (.*)")]
        public void GivenTheStartDateWhichIsTomorrowPlus(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the endDate which is tomorrow plus (.*)")]
        public void GivenTheEndDateWhichIsTomorrowPlus(int p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
