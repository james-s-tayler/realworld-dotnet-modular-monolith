/*
 * Conduit API
 *
 * Conduit API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Conduit.API.Converters;

namespace Conduit.API.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ProfileResponse : IEquatable<ProfileResponse>
    {
        /// <summary>
        /// Gets or Sets Profile
        /// </summary>
        [Required]
        [DataMember(Name="profile", EmitDefaultValue=false)]
        public Profile Profile { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProfileResponse {\n");
            sb.Append("  Profile: ").Append(Profile).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ProfileResponse)obj);
        }

        /// <summary>
        /// Returns true if ProfileResponse instances are equal
        /// </summary>
        /// <param name="other">Instance of ProfileResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProfileResponse other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Profile == other.Profile ||
                    Profile != null &&
                    Profile.Equals(other.Profile)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Profile != null)
                    hashCode = hashCode * 59 + Profile.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ProfileResponse left, ProfileResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProfileResponse left, ProfileResponse right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
