namespace TestNinja.Fundamentals
{
    public class Reservation
    {
        public User MadeBy { get; set; }

        public bool CanBeCancelledBy(User user)
        {
            return user.IsAdmin || user == MadeBy;
        }
    }

    public class User
    {
        public bool IsAdmin { get; set; }
    }
}