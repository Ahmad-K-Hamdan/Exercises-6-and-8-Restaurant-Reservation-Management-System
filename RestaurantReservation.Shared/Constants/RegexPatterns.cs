namespace RestaurantReservation.Shared.Constants
{
    public static class RegexPatterns
    {
        public const string PersonName = @"^[a-zA-Z]+$";
        public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string Phone = @"^\+?[1-9][0-9]{7,14}$";
    }
}