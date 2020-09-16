using System;
using System.Collections;
using System.Collections.Generic;
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
            var startDate = DateTime.Today.AddDays(10);
            //End date 2 days after today's date
            var endDate = DateTime.Today.AddDays(13);

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
            yield return new object[]
            {
                today.AddDays(5),
                today.AddDays(9),
                true
            };
            yield return new object[]
            {
                today.AddDays(21),
                today.AddDays(22),
                true
            };
            yield return new object[] {
                today.AddDays(11),
                today.AddDays(18),
                false
            };
            yield return new object[]
            {
                today.AddDays(5),
                today.AddDays(11),
                false
            };
            yield return new object[]
            {
                today.AddDays(18),
                today.AddDays(22),
                false
            };
            yield return new object[]
            {
                today.AddDays(3),
                today.AddDays(10),
                true
            };
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

