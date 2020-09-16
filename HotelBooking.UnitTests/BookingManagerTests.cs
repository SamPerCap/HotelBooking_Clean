using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            /////  This is an existing booking  /////
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            ///// ///// ///// ///// ///// ///// /////

            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void FindAvailableRoom_StartDateBeforeEndDateOverlap_ThrowsArgumentException()
        {
            // Start date 2 days before the existing booking
            var startDate = DateTime.Today.AddDays(8);
            // End date 3 days into the existing booking
            var endDate = DateTime.Today.AddDays(11);
            
            // Expect the FindAvailableRoom to throw an ArgumentException instead of returning an answer
            Assert.Throws<ArgumentException>(() =>
            {
                bookingManager.FindAvailableRoom(startDate, endDate);
            });
        }

        [Fact]
        public void FindAvailableRoom_StartDateOverlapsEndDateAfter_ThrowsArgumentException()
        {
            // Start date 2 days before existing booking ends
            var startDate = DateTime.Today.AddDays(18);
            // End date 3 days afetr existing booking stops
            var endDate = DateTime.Today.AddDays(23);
            
            // Expect the FindAvailableRoom to throw an ArgumentException instead of returning an answer
            Assert.Throws<ArgumentException>(() =>
            {
                bookingManager.FindAvailableRoom(startDate, endDate);
            });
        }

        [Fact]
        public void FindAvailableRoom_StartAndEndOverlapsExistingBooking_ThrowsArgumentException()
        {
            // Start date 2 days before the existing booking
            var startDate = DateTime.Today.AddDays(8);
            // End date 2 days after the existing booking
            var endDate = DateTime.Today.AddDays(22);
            
            // Expect the FindAvailableRoom to throw an ArgumentException instead of returning an answer
            Assert.Throws<ArgumentException>(() =>
            {
                bookingManager.FindAvailableRoom(startDate, endDate);
            });
        }
        
        [Fact]
        public void FindAvailableRoom_StartAndEndDatesAreInsideExistingBooking_ThrowsArgumentException()
        {
            // Start date 2 days before the existing booking
            var startDate = DateTime.Today.AddDays(12);
            // End date 2 days after the existing booking
            var endDate = DateTime.Today.AddDays(18);
            
            // Expect the FindAvailableRoom to throw an ArgumentException instead of returning an answer
            Assert.Throws<ArgumentException>(() =>
            {
                bookingManager.FindAvailableRoom(startDate, endDate);
            });

            // this is a comment
        }
        [Theory]
        [ClassData(typeof(GetFullyOccupiedDateTestData))]
        public void GetFullyOccupiedBookings_OnlyReturnBookedRooms(int startDate, int endDate, int expectedOccupiedDates)
        {

            //Arrange
            //Act
            var fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(DateTime.Today.AddDays(startDate), DateTime.Today.AddDays(endDate));

            //Assert
            Assert.Equal(expectedOccupiedDates, fullyOccupiedDates.Count);
        }
    }

    internal class GetFullyOccupiedDateTestData : TheoryData<int, int, int>
    {
        public GetFullyOccupiedDateTestData()
        {
            //startdate, enddate, expected result
            //dates are in days from today
            Add(-5, -1, 0);
            Add(0, 3, 0);
            Add(9, 11, 2); 
            Add(10, 20, 11);
            Add(17, 19, 3);
            Add(19, 25, 2);
            Add(21, 23, 0);
        }
    }
}
