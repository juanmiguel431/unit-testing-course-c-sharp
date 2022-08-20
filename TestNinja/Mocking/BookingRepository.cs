using System.Linq;

namespace TestNinja.Mocking
{
    public interface IBookingRepository
    {
        Booking GetFirstOrDefaultOverlappingBooking(Booking booking);
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }

    public class BookingRepository : IBookingRepository
    {
        public Booking GetFirstOrDefaultOverlappingBooking(Booking booking)
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Id != booking.Id && b.Status != "Cancelled");

            var overlappingBooking =
                bookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate >= b.ArrivalDate
                        && booking.ArrivalDate < b.DepartureDate
                        || booking.DepartureDate > b.ArrivalDate
                        && booking.DepartureDate <= b.DepartureDate);

            return overlappingBooking;
        }

        public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null)
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(b => b.Status != "Cancelled");

            if (excludedBookingId.HasValue)
                bookings = bookings.Where(p => p.Id != excludedBookingId);

            return bookings;
        }
    }
}