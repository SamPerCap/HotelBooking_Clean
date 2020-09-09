using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests()
        {
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

        [Fact]
        public void BookRoomAvailable_StartAndEndDatesAreInUnbookedPeriod_ExpectNotMinusOne()
        {
            //Arrange
            //Start date 1 day after today's date
            var startDate = DateTime.Today.AddDays(10);
            //End date 2 days after today's date
            var endDate = DateTime.Today.AddDays(13);

            //Act
            var actual = bookingManager.FindAvailableRoom(startDate, endDate);

            //Assert
            Assert.True(actual > -1);
        }
    }
}
