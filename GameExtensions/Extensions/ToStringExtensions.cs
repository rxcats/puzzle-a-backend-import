using System.Text;

namespace GameExtensions.Extensions
{
    public static class ToStringExtensions
    {
        public static string GetPropertiesAsText(this object obj)
        {
            var sb = new StringBuilder();

            sb.Append('{');

            foreach (var property in obj.GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append('=');
                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(obj, null));
                }
                sb.Append(", ");
            }

            return sb.ToString().TrimEnd(',', ' ') + '}';
        }
    }
}