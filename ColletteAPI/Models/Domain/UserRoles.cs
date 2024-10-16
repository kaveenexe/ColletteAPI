/*
 * File: UserRoles.cs
 * Description: This file defines a set of constants that represent the different user roles in the system.
 */

namespace ColletteAPI.Models.Domain
{
    /*
     * Class: UserRoles
     * This class contains constants that define different roles in the system (Administrator, Vendor, CSR, Customer).
     * These roles are used to assign permissions and control access to various parts of the system.
     */
    public class UserRoles
    {
        public const string Administrator = "Administrator";
        public const string Vendor = "Vendor";
        public const string CSR = "CSR";
        public const string Customer = "Customer";
    }
}
