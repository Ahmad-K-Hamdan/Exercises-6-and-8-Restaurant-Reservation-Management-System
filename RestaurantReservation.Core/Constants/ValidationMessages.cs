namespace RestaurantReservation.Core.Constants
{
    public static class ValidationMessages
    {
        // Required Field Messages
        public const string FirstNameRequired = "First name is required.";
        public const string LastNameRequired = "Last name is required.";
        public const string EmailRequired = "Email is required.";
        public const string PhoneNumberRequired = "Phone number is required.";
        public const string RestaurantNameRequired = "Restaurant name is required.";
        public const string AddressRequired = "Address is required.";
        public const string OpeningHoursRequired = "Opening hours are required.";
        public const string PositionRequired = "Position is required.";
        public const string MenuItemNameRequired = "Menu item name is required.";
        public const string DescriptionRequired = "Description is required.";
        public const string ReservationDateRequired = "Reservation date is required.";
        public const string OrderDateRequired = "Order date is required.";
        public const string InputCannotBeEmpty = "Input cannot be empty. Please try again.";

        // Name Validation Messages
        public const string NameTooShort = "Name must be at least 2 characters long.";
        public const string NameTooLong = "Name cannot exceed 50 characters.";
        public const string NameInvalidCharacters = "Name can only contain letters.";
        public const string RestaurantNameTooShort = "Restaurant name must be at least 2 characters long.";
        public const string RestaurantNameTooLong = "Restaurant name cannot exceed 100 characters.";
        public const string MenuItemNameTooShort = "Menu item name must be at least 2 characters long.";
        public const string MenuItemNameTooLong = "Menu item name cannot exceed 50 characters.";

        // Contact Validation Messages
        public const string EmailInvalid = "Please enter a valid email address.";
        public const string EmailMaxLength = "Email cannot exceed 100 characters.";
        public const string EmailAlreadyInUse = "Email address is already in use.";
        public const string PhoneInvalid = "Please enter a valid phone number.";

        // Address Validation Messages
        public const string AddressTooShort = "Address must be at least 5 characters long.";
        public const string AddressTooLong = "Address cannot exceed 200 characters.";

        // Description Validation Messages
        public const string DescriptionTooShort = "Description must be at least 10 characters long.";
        public const string DescriptionTooLong = "Description cannot exceed 500 characters.";

        // Date and Time Validation Messages
        public const string InvalidDate = "Please enter a valid date in yyyy-mm-dd hh:mm format.";
        public const string DateCannotBePast = "Date cannot be in the past.";
        public const string DateCannotBeFuture = "Date cannot be in the future.";
        public const string ReservationDateTooFar = "Reservation date cannot be more than 1 year in the future.";
        public const string DateTooFar = "Date cannot be more than 1 year in the future.";
        public const string DateTooOld = "Date cannot be more than 1 year old.";
        public const string OrderDateTooOld = "Order date cannot be more than 1 day old.";
        public const string TimeInvalid = "Please enter time in HH:mm format.";
        public const string TimeOutOfRange = "Time must be between 00:00 and 23:59.";
        public const string OpeningHoursInvalidFormat = "Opening hours must be in format 'HH:mm-HH:mm' (e.g., '09:00-21:00').";
        public const string OpeningHoursInvalidRange = "Opening time must be before closing time.";

        // Numeric Validation Messages
        public const string InvalidNumber = "Please enter a valid number.";
        public const string CapacityLessThanOne = "Capacity must be at least 1.";
        public const string CapacityTooHigh = "Capacity cannot exceed 50.";
        public const string QuantityLessThanOrEqualToZero = "Quantity must be greater than 0.";
        public const string QuantityTooHigh = "Quantity cannot exceed 100.";
        public const string PartySizeLessThanOrEqualToZero = "Party size must be greater than 0.";
        public const string PartySizeTooHigh = "Party size cannot exceed 50.";
        public const string PriceLessThanOrEqualToZero = "Price must be greater than 0.";
        public const string PriceTooHigh = "Price cannot exceed 1000.";
        public const string TotalAmountLessThanOrEqualToZero = "Total amount must be greater than 0.";
        public const string TotalAmountTooHigh = "Total amount cannot exceed 10000.";

        // Logic Validation Messages
        public const string PositionInvalid = "Position must be one of: Manager, Server, Chef, Host, Bartender, Cashier.";
        public const string PartySizeExceedsTableCapacity = "Party size exceeds table capacity.";
        public const string TableAlreadyReserved = "This table is already reserved for the selected time slot (within 2 hours).";
        public const string PriceNotReasonable = "Price is significantly different from other items in this restaurant (more than 5x or less than 0.2x the average).";
        public const string OrderDateMustMatchReservation = "Order date must be on the same day as the reservation date.";

        // Foreign Key Validation Messages
        public const string ValidRestaurantIdRequired = "Valid restaurant ID is required.";
        public const string ValidCustomerIdRequired = "Valid customer ID is required.";
        public const string ValidEmployeeIdRequired = "Valid employee ID is required.";
        public const string ValidTableIdRequired = "Valid table ID is required.";
        public const string ValidReservationIdRequired = "Valid reservation ID is required.";
        public const string ValidOrderIdRequired = "Valid order ID is required.";
        public const string ValidItemIdRequired = "Valid item ID is required.";

        // Entity Not Found Messages
        public const string RestaurantNotFound = "Restaurant not found. Please enter a valid restaurant ID.";
        public const string CustomerNotFound = "Customer not found. Please enter a valid customer ID.";
        public const string EmployeeNotFound = "Employee not found. Please enter a valid employee ID.";
        public const string TableNotFound = "Table not found. Please enter a valid table ID.";
        public const string MenuItemNotFound = "Menu item not found. Please enter a valid menu item ID.";
        public const string OrderNotFound = "Order not found. Please enter a valid order ID.";
        public const string OrderItemNotFound = "Order item not found. Please enter a valid order item ID.";
        public const string ReservationNotFound = "Reservation not found. Please enter a valid reservation ID.";
    }
}