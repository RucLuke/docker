using System;

namespace User.API.Models
{
    public class UserProperty
    {
        private int? _requestHashCode;
        /// <summary>
        /// 
        /// </summary>
        public int AppUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is UserProperty))
                return false;

            if (ReferenceEquals(this, obj))
                return true;
            var item = (UserProperty)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;

            return item.Key == this.Key && item.Value == this.Value;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestHashCode.HasValue)
                    _requestHashCode = (Key + Value).GetHashCode() ^ 31;
                return _requestHashCode.Value;
            }

            return base.GetHashCode();

        }

        public static bool operator ==(UserProperty left, UserProperty right)
        {
            return left?.Equals(right) ?? object.Equals(right, null);
        }

        public static bool operator !=(UserProperty left, UserProperty right)
        {
            return !(left == right);
        }

        private bool IsTransient()
        {
            return string.IsNullOrEmpty(this.Key) || string.IsNullOrEmpty(this.Value);
        }


    }
}
