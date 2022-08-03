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
    public partial class User : IEquatable<User>
    {
        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [Required]
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets Token
        /// </summary>
        [Required]
        [DataMember(Name = "token", EmitDefaultValue = false)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [Required]
        [DataMember(Name = "username", EmitDefaultValue = false)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets Bio
        /// </summary>
        [Required]
        [DataMember(Name = "bio", EmitDefaultValue = false)]
        public string Bio { get; set; }

        /// <summary>
        /// Gets or Sets Image
        /// </summary>
        [Required]
        [DataMember(Name = "image", EmitDefaultValue = false)]
        public string Image { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class User {\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("  Bio: ").Append(Bio).Append("\n");
            sb.Append("  Image: ").Append(Image).Append("\n");
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
            if ( obj is null ) return false;
            if ( ReferenceEquals(this, obj) ) return true;
            return obj.GetType() == GetType() && Equals(( User )obj);
        }

        /// <summary>
        /// Returns true if User instances are equal
        /// </summary>
        /// <param name="other">Instance of User to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(User other)
        {
            if ( other is null ) return false;
            if ( ReferenceEquals(this, other) ) return true;

            return
                (
                    Email == other.Email ||
                    Email != null &&
                    Email.Equals(other.Email)
                ) &&
                (
                    Token == other.Token ||
                    Token != null &&
                    Token.Equals(other.Token)
                ) &&
                (
                    Username == other.Username ||
                    Username != null &&
                    Username.Equals(other.Username)
                ) &&
                (
                    Bio == other.Bio ||
                    Bio != null &&
                    Bio.Equals(other.Bio)
                ) &&
                (
                    Image == other.Image ||
                    Image != null &&
                    Image.Equals(other.Image)
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
                if ( Email != null )
                    hashCode = hashCode * 59 + Email.GetHashCode();
                if ( Token != null )
                    hashCode = hashCode * 59 + Token.GetHashCode();
                if ( Username != null )
                    hashCode = hashCode * 59 + Username.GetHashCode();
                if ( Bio != null )
                    hashCode = hashCode * 59 + Bio.GetHashCode();
                if ( Image != null )
                    hashCode = hashCode * 59 + Image.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(User left, User right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
