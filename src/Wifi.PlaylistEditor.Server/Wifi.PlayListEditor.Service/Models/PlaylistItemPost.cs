
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Wifi.PlayListEditor.Service.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlaylistItemPost 
    {
        /// <summary>
        /// The id of the item to Updates
        /// </summary>
        /// <value>The id of the item to Updates</value>
        [Required]
        [DataMember(Name = "id")]
        public string Id { get; set; }

       
       
    }
}
