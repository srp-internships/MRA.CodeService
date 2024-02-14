namespace Core.ValidationsBehaviours
{
    public static class ValidationMessages
    {
        public static string GetNotNullMessage(string propertyName)
        {
            return $"'{propertyName}' не должно быть null";
        }

        public static string GetNotEmptyMessage(string propertyName)
        {
            return $"'{propertyName}' не должно быть пустым";
        }

        public static string GetEmailAddressMessage()
        {
            return $"Требуется правильный адрес электронной почты";
        }

        public static string GetGreaterThanMessage(string startDate)
        {
            return $"Дата окончания должна быть больше, чем {startDate}";
        }

        public static string GetGreaterThanOrEqualToMessage(string propertyName, int min)
        {
            return $"'{propertyName}' должен быть больше или равен {min}";
        }

        public static string GetLessThanOrEqualToMessage(string propertyName, int max)
        {
            return $"'{propertyName}' должен быть меньше или равен {max}";
        }
    }
}
