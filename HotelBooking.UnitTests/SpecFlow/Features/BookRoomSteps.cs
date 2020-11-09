using System;
using TechTalk.SpecFlow;

namespace HotelBooking.UnitTests.SpecFlow.Features
{
    [Binding]
    public class BookRoomSteps
    {
        [Given(@"the startDate is tomorrow(.*)")]
        public void GivenTheStartDateIsTomorrow(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"the endDate is tomorrow(.*)")]
        public void GivenTheEndDateIsTomorrow(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"the startDate is tomorrow\+(.*)")]
        public void GivenTheStartDateIsTomorrow(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"the endDate is tomorrow\+(.*)")]
        public void GivenTheEndDateIsTomorrow(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the dates are check")]
        public void WhenTheDatesAreCheck()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be false")]
        public void ThenTheResultShouldBeFalse()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
