using System;
using System.Collections;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
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

            var bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=2, RoomId=2 },
            };
            
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            mockBookingRepository
                .Setup(x => x.GetAll())
                .Returns(bookings);

            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };
            var mockRoomRepository = new Mock<IRepository<Room>>();
            mockRoomRepository
                .Setup(x => x.GetAll())
                .Returns(rooms);

            var bookingRepository = mockBookingRepository.Object;
            var roomRepository = mockRoomRepository.Object;
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));
        }

        //test
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
            // Start date 6 days before the existing booking
            var startDate = DateTime.Today.AddDays(6);
            // End date 10 days into the existing booking
            var endDate = DateTime.Today.AddDays(12);

            // Expect the FindAvailableRoom to throw an ArgumentException instead of returning an answer
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, roomId);
        }

        [Fact]
        public void FindAvailableRoom_StartDateOverlapsEndDateAfter_ThrowsArgumentException()
        {
            // Start date 2 days before existing booking ends
            var startDate = DateTime.Today.AddDays(18);
            // End date 3 days afetr existing booking stops
            var endDate = DateTime.Today.AddDays(23);

            // Expect the FindAvailableRoom to throw an ArgumentException instead of returning an answer
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, roomId);
        }

        [Fact]
        public void FindAvailableRoom_StartAndEndOverlapsExistingBooking_ThrowsArgumentException()
        {
            // Start date 8 days before the existing booking
            var startDate = DateTime.Today.AddDays(8);
            // End date 22 days after the existing booking
            var endDate = DateTime.Today.AddDays(22);

            // Expect the FindAvailableRoom and return false
            var actual = bookingManager.FindAvailableRoom(startDate, endDate);

            Assert.True(actual.Equals(-1));
        }

        [Fact]
        public void FindAvailableRoom_StartAndEndDatesAreInsideExistingBooking_ThrowsArgumentException()
        {
            // Start date 11 days before the existing booking
            var startDate = DateTime.Today.AddDays(11);
            // End date 18 days after the existing booking
            var endDate = DateTime.Today.AddDays(18);
            
            // Expect the FindAvailableRoom and throw no available (false (-1))
            var actual = bookingManager.FindAvailableRoom(startDate, endDate);
            
            Assert.True(actual.Equals(-1));
        }

        [Theory]
        [ClassData(typeof(CreateBookTest))]
        public void CreateBook_StartAndEndDateAreAvailable_ExpectTrue(DateTime startDate, DateTime endDate, bool expected)
        {
            Booking bking = new Booking();
            bking.StartDate = startDate;
            bking.EndDate = endDate;
            var result = bookingManager.CreateBooking(bking);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BookRoomAvailable_StartAndEndDatesAreInUnbookedPeriod_ExpectNotMinusOne()
        {
            //Arrange
            //Start date 1 day after today's date
            var startDate = DateTime.Today.AddDays(1);
            //End date 3 days after today's date
            var endDate = DateTime.Today.AddDays(3);

            //Act
            var actual = bookingManager.FindAvailableRoom(startDate, endDate);

            //Assert
            Assert.True(actual > -1);
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


    //Internal classes
    internal class CreateBookTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            DateTime today = DateTime.Today;
            //Booking = 10-20

            // Starts and end before the booking dates
            yield return new object[]
            {
                today.AddDays(5),
                today.AddDays(9),
                true
            };
            // Starts and end after the booking dates
            yield return new object[]
            {
                today.AddDays(21),
                today.AddDays(22),
                true
            };
            // Start and end inside the booking days
            yield return new object[] {
                today.AddDays(11),
                today.AddDays(18),
                false
            };
            // Start out of the booking days and ends inside it
            yield return new object[]
            {
                today.AddDays(5),
                today.AddDays(11),
                false
            };
            // Starts inside the booking days and ends out
            yield return new object[]
            {
                today.AddDays(18),
                today.AddDays(22),
                false
            };
            // It ends the same day the booking starts
            yield return new object[]
            {
                today.AddDays(3),
                today.AddDays(10),
                true
            };
            // Starts the same day the booking ends. Ends later
            yield return new object[] {
                today.AddDays(20),
                today.AddDays(25),
                true
            };

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class GetFullyOccupiedDateTestData : TheoryData<int, int, int>
    {
        public GetFullyOccupiedDateTestData()
        {

            //startdate, enddate, expected result
            //dates are in days from today
            Add(20, 23, 1); // available and successfully book 1 
            Add(10, 19, 10); // dates occupied between these dates

        }
    }

    /// <summary>
    /// Example of Class Data, following Henrik's example
    /// </summary>
    internal class GetFullyOccupiedDateTestDataEnumerable : IEnumerable<object[]>
    {
        private readonly List<object[]> list = new List<object[]>
        {
            new object[]{0,0,0},
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

