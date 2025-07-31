using System.Net.Mail;

namespace Insurance.Propost.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        public string Address { get; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email cannot be empty.");

            if (!IsValidEmail(address))
                throw new ArgumentException("Invalid email format.");

            Address = address;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Address;

        public override bool Equals(object? obj) => Equals(obj as Email);

        public bool Equals(Email? other) => other is not null && Address == other.Address;

        public override int GetHashCode() => Address.GetHashCode();

        public static implicit operator string(Email email) => email.Address;
        public static explicit operator Email(string email) => new(email);
    }
}
