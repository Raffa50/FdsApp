using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fds.Models.Mobile{
    public class UserConverter : JsonConverter {
        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            var u= new User( (IUser) value );
            serializer.Serialize(writer,u);
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            return serializer.Deserialize< User >( reader );
        }
        public override bool CanConvert( Type objectType ) { return objectType == typeof( IUser ); }
    }
}
