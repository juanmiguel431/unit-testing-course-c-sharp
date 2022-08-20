﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking
{
    public static class BookingHelper
    {
        public static IBookingRepository BookingRepository;
        
        public static string OverlappingBookingsExist(Booking booking)
        {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var overlappingBooking = BookingRepository.GetFirstOrDefaultOverlappingBooking(booking);

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }

    public class UnitOfWork
    {
        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }
    }

    public class Booking
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
}