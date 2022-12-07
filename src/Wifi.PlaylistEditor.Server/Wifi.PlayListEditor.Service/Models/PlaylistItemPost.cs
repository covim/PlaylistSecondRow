/*
 * API description for Playlist project
 *
 * This is a sample Playlist Server based on the OpenAPI 3.0 specification.  You can find out more about OpenAPI at [https://oai.github.io/Documentation](https://oai.github.io/Documentation).      Playlist server should provide following functionalities: - Upload item to server - Delete item from server - Get data from one item - Get list of all items on server - Create playlists - Modify playlists - Get a list of all existing playlists Further sources for information   - [Multipart Requests](https://swagger.io/docs/specification/describing-request-body/multipart-requests)   - [Upload And Download Multiple Files Using Web API](https://github.com/JayKrishnareddy/UploadandDownloadFiles)
 *
 * OpenAPI spec version: 1.0.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Wifi.PlayListEditor.Service.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlaylistItemPost : IEquatable<PlaylistItemPost>
    {
        /// <summary>
        /// The id of the item to Updates
        /// </summary>
        /// <value>The id of the item to Updates</value>
        [Required]

        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Filename
        /// </summary>

        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        /// <summary>
        /// Gets or Sets Extension
        /// </summary>

        [DataMember(Name = "extension")]
        public string Extension { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PlaylistItemPost {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Filename: ").Append(Filename).Append("\n");
            sb.Append("  Extension: ").Append(Extension).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((PlaylistItemPost)obj);
        }

        /// <summary>
        /// Returns true if PlaylistItemPost instances are equal
        /// </summary>
        /// <param name="other">Instance of PlaylistItemPost to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PlaylistItemPost other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) &&
                (
                    Filename == other.Filename ||
                    Filename != null &&
                    Filename.Equals(other.Filename)
                ) &&
                (
                    Extension == other.Extension ||
                    Extension != null &&
                    Extension.Equals(other.Extension)
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
                if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                if (Filename != null)
                    hashCode = hashCode * 59 + Filename.GetHashCode();
                if (Extension != null)
                    hashCode = hashCode * 59 + Extension.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(PlaylistItemPost left, PlaylistItemPost right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PlaylistItemPost left, PlaylistItemPost right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
