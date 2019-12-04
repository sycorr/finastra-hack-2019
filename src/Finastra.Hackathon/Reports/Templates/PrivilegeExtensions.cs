using System;

namespace Finastra.Hackathon.Reports.Templates
{
    public static class PrivilegeExtensions
    {
        public static string ToConfiguredDisplay(this string privilege)
        {
            if (String.IsNullOrEmpty(privilege))
                return privilege;

            var ro = "<span class=\"label label-solid label-primary privilege-label mtt\" style=\"margin-left:7px\">RO</span>";
            var rw = "<span class=\"label label-solid label-success privilege-label mtt\" style=\"margin-left:7px\">RW</span>";

            var inner = privilege.Replace(" [RO]", "").Replace(" [RW]", "");

            if (privilege.Contains(" [RO]"))
                return inner + ro;

            if (privilege.Contains(" [RW]"))
                return inner + rw;

            return inner;
        }
    }
}